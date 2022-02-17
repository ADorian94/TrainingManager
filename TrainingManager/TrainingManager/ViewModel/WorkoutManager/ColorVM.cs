using System;
using System.Collections.ObjectModel;
using TrainingManager.Data;
using TrainingManager.Model;

namespace TrainingManager.ViewModel
{
    public class ColorVM : ViewModelBase
    {
        //PROPERTIES
        private ObservableCollection<MaterialColorName> _colors;
        public ObservableCollection<MaterialColorName> Colors { get => _colors; set { _colors = value; OnPropertyChanged(); } }

        //COMMAND
        public DelegateCommand ColorSelectedCommand { get; set; }

        //EVENT
        public event EventHandler<MaterialColors> ColorSelected;

        public ColorVM()
        {
            Colors = new ObservableCollection<MaterialColorName>();

            foreach (MaterialColors color in Enum.GetValues(typeof(MaterialColors)))
                Colors.Add(new MaterialColorName()
                {
                    Color = color,
                    Name = GetColorName(color)
                });
        }

        private void ColorSelectedFunction(object obj)
        {
            bool result = Enum.TryParse((string)obj, out MaterialColors Parsedcolor);
            MaterialColors color = result ? Parsedcolor : MaterialColors.Default;
            ColorSelected?.Invoke(this, color);
        }

        private string GetColorName(MaterialColors color)
        {
            switch (color)
            {
                case MaterialColors.Default:
                    return "Default";
                case MaterialColors.Red:
                    return "Red";
                case MaterialColors.Purple:
                    return "Purple";
                case MaterialColors.DeepPurple:
                    return "Deep Purple";
                case MaterialColors.Blue:
                    return "Blue";
                case MaterialColors.Cyan:
                    return "Cyan";
                case MaterialColors.LightGreen:
                    return "Light Green";
                case MaterialColors.Lime:
                    return "Lime";
                case MaterialColors.Amber:
                    return "Amber";
                case MaterialColors.DeepOrange:
                    return "Deep Orange";
                case MaterialColors.Brown:
                    return "Brown";
                default:
                    throw new NotImplementedException();
            }
        }

        protected override void InitializeCommands()
        {
            ColorSelectedCommand = new DelegateCommand(ColorSelectedFunction);
        }
    }
}
