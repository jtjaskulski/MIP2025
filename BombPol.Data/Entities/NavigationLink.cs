namespace BombPol.Data.Entities
{
    public class NavigationLink : SoftDeleteBusinessModel
    {
        public string LinkTitle { get; set; }
        public string? ControllerName { get; set; }
        public string? ControllerAction { get; set; }
        public string? Url { get; set; }

        public NavigationLink()
            : base(Guid.Empty)
        {
            
        }
        public NavigationLink(Guid id, string linkTitle, string controllerName, string controllerAction, string url)
            :base(id)
        {
                LinkTitle = linkTitle;
                ControllerName = controllerName ?? string.Empty;
                ControllerAction = controllerAction ?? string.Empty;
                Url = url ?? string.Empty;
        }
    }
}
