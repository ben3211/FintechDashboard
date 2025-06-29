namespace FintechDashboard.Web.Shared
{
    public partial class NavMenu
    {
        private bool collapseNavMenu = false;
        private string? NavMenuCssClass => collapseNavMenu ? "show" : null;
        
        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}
