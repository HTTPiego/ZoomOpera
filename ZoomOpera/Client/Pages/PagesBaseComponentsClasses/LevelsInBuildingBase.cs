using Microsoft.AspNetCore.Components;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Pages.PagesBaseComponentsClasses
{
    public class LevelsInBuildingBase : ComponentBase, IDisposable
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IService<ILevel, LevelDTO> Service { get; set; }

        [Inject]
        protected IEventHandler<ILevel> Handler { get; set; }

        [Parameter]
        public Guid FatherBuildingId { get; set; }

        //[Parameter]
        //public LevelDTO Level { get; set; } = new LevelDTO();

        public List<ILevel> Levels { get; set; } = new List<ILevel>();

        public async Task<IEnumerable<ILevel>> GetLevels()
        {
            return await Service.GetAllByfatherRelationshipId(FatherBuildingId);
        }

        public void GoToAddLevel()
        {
            NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani/aggiungi-piano");
        }

        public void GoToLocations(Guid id)
        {
            NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani/{id}/locazioni-opere");
        }

        public void GoToMonitofPLatforms(Guid id)
        {
            NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani/{id}/piattaforme-monitor");
        }

        public void ModifyLevel(Guid id)
        {
            NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani/{id}/modifica-piano");
        }

        public async void DeleteLevel(Guid id)
        {
            var deletedLevel = await Service.DeleteEntity(id);
            Levels.Remove(deletedLevel);
            Levels.OrderBy(l => l.LevelNumber);
            StateHasChanged();
        }

        public void HandleOnAddedLevel(ILevel addedLevel)
        {
            Levels.Add(addedLevel);
            Levels.OrderBy(l => l.LevelNumber);
        }

        public void HandleOnModifiedAddedLevel(object sender, ILevel modifiedLevel)
        {
            var index = Levels.FindIndex(l => l.Id.Equals(modifiedLevel.Id));

            if (index != -1)
                Levels[index] = modifiedLevel;
            else
                Levels.Add(modifiedLevel);

            Levels.OrderBy(l => l.LevelNumber);
        }

        protected override async Task OnInitializedAsync()
        {
            var levels = await GetLevels();
            Levels = levels.ToList();
            Levels.OrderBy(l => l.LevelNumber);
            Handler.EventHandler += HandleOnModifiedAddedLevel;
        }

        public void Dispose()
        {
            Handler.EventHandler -= HandleOnModifiedAddedLevel;
        }
    }
}
