// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Xunit.v3;

namespace Xunit;

/// <summary>
///  A local member data attribute for legacy tests so they don't depend on the shared test utilities project.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class CommonMemberDataAttribute : MemberDataAttributeBase
{
    public CommonMemberDataAttribute(Type memberType, string memberName = "TheoryData")
        : this(memberType, memberName, [])
    {
    }

    public CommonMemberDataAttribute(Type memberType, string memberName, params object?[] parameters)
        : base(memberName, parameters)
    {
        MemberType = memberType;
    }
}
