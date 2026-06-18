// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections;
using System.Drawing;
using System.Text;

namespace WinFormsControlsTest;

// Minimal, self-contained repro for the TreeView AddRange ordering regression.
//
// Scenario (mirrors a real application that builds a sorted TreeView):
//   * TreeView.Sorted == true with a custom TreeViewNodeSorter.
//   * The sorter reports every node as equal (Compare == 0) - e.g. several
//     identically described items.
//   * The nodes are added in one shot via TreeNodeCollection.AddRange.
//
// Expected: the equal nodes keep their input order (Pallet 1, Pallet 2, ...).
// Observed (.NET 9+): the equal nodes come back reversed (Pallet N, ..., Pallet 1).
//
// This file also exposes a headless "report" mode so the exact same build can be
// launched in a child process with the AppContext switch flipped, proving whether
// the switch restores the old behavior. See Program.cs for the command-line hook.
[DesignerCategory("code")]
internal sealed class TreeViewSortAddRangeTest : Form
{
    // The AppContext switch added by dotnet/winforms PR #11423.
    // Default value is true ("respect sort order") on .NET 8+.
    internal const string SwitchName = "System.Windows.Forms.TreeNodeCollectionAddRangeRespectsSortOrder";

    private const int PalletCount = 5;
    private const string ReportArg = "--treeview-addrange-report";

    private readonly TreeView _treeView;
    private readonly TextBox _output;
    private readonly Label _switchLabel;

    public TreeViewSortAddRangeTest()
    {
        Text = "TreeView - AddRange sort order repro";
        ClientSize = new Size(760, 460);

        _switchLabel = new Label
        {
            Dock = DockStyle.Top,
            AutoSize = false,
            Height = 48,
            Padding = new Padding(8, 8, 8, 0),
            Text = DescribeEffectiveSwitch()
        };

        _treeView = new TreeView
        {
            Dock = DockStyle.Left,
            Width = 240,
            Sorted = true,
            TreeViewNodeSorter = new AlwaysEqualComparer()
        };

        _output = new TextBox
        {
            Dock = DockStyle.Fill,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = System.Windows.Forms.ScrollBars.Both,
            WordWrap = false,
            Font = new Font(FontFamily.GenericMonospace, 9f)
        };

        FlowLayoutPanel buttons = new()
        {
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.LeftToRight,
            AutoSize = true,
            Padding = new Padding(4)
        };

        Button reproButton = new() { Text = "Reproduce (AddRange)", AutoSize = true };
        reproButton.Click += (s, e) => ShowScenario(useAddRange: true);

        Button addButton = new() { Text = "Contrast (Add one-by-one)", AutoSize = true };
        addButton.Click += (s, e) => ShowScenario(useAddRange: false);

        Button compareButton = new() { Text = "Compare switch ON / OFF (child processes)", AutoSize = true };
        compareButton.Click += (s, e) => CompareSwitchInChildProcesses();

        buttons.Controls.Add(reproButton);
        buttons.Controls.Add(addButton);
        buttons.Controls.Add(compareButton);

        Controls.Add(_output);
        Controls.Add(_treeView);
        Controls.Add(buttons);
        Controls.Add(_switchLabel);

        ShowScenario(useAddRange: true);
    }

    private void ShowScenario(bool useAddRange)
    {
        TreeNode[] nodes = CreatePalletNodes(PalletCount);

        _treeView.BeginUpdate();
        _treeView.Nodes.Clear();
        if (useAddRange)
        {
            _treeView.Nodes.AddRange(nodes);
        }
        else
        {
            foreach (TreeNode node in nodes)
            {
                _treeView.Nodes.Add(node);
            }
        }

        _treeView.EndUpdate();

        string method = useAddRange ? "AddRange(nodes)" : "Add(node) in a loop";
        string output = JoinNodeText(_treeView.Nodes.Cast<TreeNode>());
        string input = JoinNodeText(nodes);

        StringBuilder sb = new();
        sb.AppendLine(DescribeEffectiveSwitch());
        sb.AppendLine();
        sb.AppendLine($"Insert method : {method}");
        sb.AppendLine($"Input:  {input}");
        sb.AppendLine($"Output: {output}");
        sb.AppendLine($"Reversed?     : {(output == input ? "no (stable)" : "YES - equal nodes reversed")}");
        _output.Text = sb.ToString();
    }

    private void CompareSwitchInChildProcesses()
    {
        string self = Environment.ProcessPath;
        if (self is null)
        {
            _output.Text = "Could not resolve the current process path.";
            return;
        }

        StringBuilder sb = new();
        sb.AppendLine("Each line below is produced by a SEPARATE child process of this exact build,");
        sb.AppendLine("so the AppContext switch is applied cleanly before the first AddRange call.");
        sb.AppendLine();
        sb.AppendLine(RunChild(self, $"{ReportArg} --switch:false"));
        sb.AppendLine(RunChild(self, $"{ReportArg} --switch:true"));
        _output.Text = sb.ToString();
    }

    private static string RunChild(string fileName, string arguments)
    {
        try
        {
            ProcessStartInfo psi = new()
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using Process process = Process.Start(psi);
            if (process is null)
            {
                return $"[{arguments}] failed to start";
            }

            string stdout = process.StandardOutput.ReadToEnd();
            process.WaitForExit(30_000);
            return stdout.Trim();
        }
        catch (Exception ex)
        {
            return $"[{arguments}] error: {ex.Message}";
        }
    }

    private static TreeNode[] CreatePalletNodes(int count)
    {
        TreeNode[] nodes = new TreeNode[count];
        for (int i = 0; i < count; i++)
        {
            nodes[i] = new TreeNode($"Pallet {i + 1}") { Name = $"Pallet{i + 1}" };
        }

        return nodes;
    }

    private static string DescribeEffectiveSwitch()
    {
        if (AppContext.TryGetSwitch(SwitchName, out bool value))
        {
            return $"AppContext switch '{SwitchName}' = {value} (explicitly set)";
        }

        return $"AppContext switch '{SwitchName}' = <not set> (framework default: true)";
    }

    /// <summary>
    ///  Headless entry point used by the child processes spawned from
    ///  <see cref="CompareSwitchInChildProcesses"/> and by command-line runs.
    ///  Returns true when the arguments requested the report (and it was printed).
    /// </summary>
    internal static bool TryRunConsoleReport(string[] args, out int exitCode)
    {
        exitCode = 0;
        if (args is null || !args.Any(a => string.Equals(a, ReportArg, StringComparison.OrdinalIgnoreCase)))
        {
            return false;
        }

        string switchArg = args.FirstOrDefault(a => a.StartsWith("--switch:", StringComparison.OrdinalIgnoreCase));
        if (switchArg is not null && bool.TryParse(switchArg["--switch:".Length..], out bool switchValue))
        {
            // Must be set before the first time the switch is read (first sorted AddRange).
            AppContext.SetSwitch(SwitchName, switchValue);
        }

        TreeNode[] nodes = CreatePalletNodes(PalletCount);
        using TreeView treeView = new() { Sorted = true, TreeViewNodeSorter = new AlwaysEqualComparer() };
        treeView.CreateControl();
        treeView.Nodes.AddRange(nodes);

        string output = JoinNodeText(treeView.Nodes.Cast<TreeNode>());
        string input = JoinNodeText(nodes);
        string state = AppContext.TryGetSwitch(SwitchName, out bool v)
            ? (v ? "ON" : "OFF")
            : "<default:true>";

        Console.Out.WriteLine($"switch {state} ->");
        Console.Out.WriteLine($"Input:  {input}");
        Console.Out.WriteLine($"Output: {output}");
        Console.Out.WriteLine($"Reversed: {output != input}");
        return true;
    }

    private static string JoinNodeText(IEnumerable<TreeNode> nodes) => string.Join(",", nodes.Select(n => n.Text));

    // A sorter that treats every node as equal, like several identically described items.
    private sealed class AlwaysEqualComparer : IComparer
    {
        public int Compare(object x, object y) => 0;
    }
}
