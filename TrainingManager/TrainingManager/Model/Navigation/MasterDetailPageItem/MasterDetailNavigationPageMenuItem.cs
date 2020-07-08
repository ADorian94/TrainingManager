using System;
using TrainingManager.View;

namespace TrainingManager.Model.Navigation.MasterDetailPageItem
{

    public class MasterDetailNavigationPageMenuItem
    {
        public MasterDetailNavigationPageMenuItem()
        {
            TargetType = typeof(MainPage);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public Type TargetType { get; set; }
    }
}