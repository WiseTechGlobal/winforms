// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System.ComponentModel;
using System.Windows.Forms;

namespace WTG.System.Windows.Forms.System.Windows.Forms
{
    public class ErrorProviderTests
    {
        public static IEnumerable<object[]> CanExtend_TestData()
        {
            yield return new object[] { null, false };
            yield return new object[] { new(), false };
            yield return new object[] { new Component(), false };
            yield return new object[] { new ToolBar(), false };
            yield return new object[] { new Form(), false };
            yield return new object[] { new Control(), true };
        }

        [Theory]
        [MemberData(nameof(CanExtend_TestData))]
        public void ErrorProvider_CanExtend_Invoke_ReturnsExpected(object extendee, bool expected)
        {
            var provider = new ErrorProvider();
            Assert.Equal(expected, provider.CanExtend(extendee));
        }
    }
}
