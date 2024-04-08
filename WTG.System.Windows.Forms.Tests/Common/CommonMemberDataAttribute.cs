using System.Reflection;

namespace WTG.System.Windows.Forms.Tests.Common
{
    /// <summary>
    ///  A custom MemberData attribute that is specialized for the CommonTestHelper type.
    ///  Useful to remove the need to suffix all attributes with "MemberType = ..."
    ///  We cannot inherit from MemberDataAttribute as it is sealed, so we have to reimplement
    ///  ConvertDataItem inheriting from MemberDataAttributeBase.
    /// </summary>
    public sealed class CommonMemberDataAttribute : MemberDataAttributeBase
    {
        public CommonMemberDataAttribute(string memberName, params object[] parameters) : base(memberName, parameters)
        {
            MemberType = typeof(CommonTestHelper);
        }

        protected override object[] ConvertDataItem(MethodInfo testMethod, object item)
        {
            if (item is null)
            {
                return null;
            }

            if (!(item is object[] array))
            {
                throw new ArgumentException($"Property {MemberName} on {MemberType ?? testMethod.DeclaringType} yielded an item that is not an object[]");
            }

            return array;
        }
    }
}
