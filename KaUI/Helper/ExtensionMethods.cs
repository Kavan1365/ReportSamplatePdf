namespace KaUI.Helper
{
    public static class ExtensionMethods
    {

        public class ModelShowAttribute : System.Attribute
        {
            public bool IsLast { get; set; }
            public string EventName { get; set; }

            public ModelShowAttribute(bool isLast, string eventName)
            {
                this.IsLast = isLast;
                this.EventName = eventName;
            }
        }

        public class CreateIconAttribute : System.Attribute
        {
            public bool IsEnable { get; set; }
            public string Icon { get; set; }
            public string Title { get; set; }

            public CreateIconAttribute(bool IsEnable, string Icon, string Title)
            {
                this.IsEnable = IsEnable;
                this.Icon = Icon;
                this.Title = Title;
            }
        }

    }
}
