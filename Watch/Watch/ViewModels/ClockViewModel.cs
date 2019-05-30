using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Watch.ViewModels
{
    public class ClockViewModel : ViewModelBase
    {
        public ClockViewModel(INavigationService navigationService)
            : base(navigationService)
        {
        }
    }
}
