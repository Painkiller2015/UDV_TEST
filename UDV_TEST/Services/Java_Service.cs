using UDV_TEST.Entity;

namespace UDV_TEST.Services
{
    public static class Java_Service
    {
        public static class CastJavaObject
        {
            public static T Cast<T>(Java.Lang.Object obj) where T : Chat_List_item
            {
                var propInfo = obj.GetType().GetProperty("Instance");
                return propInfo == null ? null : propInfo.GetValue(obj, null) as T;
            }
        }
    }
}
