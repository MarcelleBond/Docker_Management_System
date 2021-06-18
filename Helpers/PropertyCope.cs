namespace Night_Shadow.Helpers
{
    public static class PropertyCope
    {
        public static void Copy<T>(this T destination, T source)
        {
            var props = destination.GetType().GetProperties();
            foreach (var prop in props)
            {
                prop.SetValue(destination, prop.GetValue(source));
            }
        }
    }
}