using Prism.Navigation;
using Reactive.Bindings;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using SQLiteSample.Models;

namespace SQLiteSample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {

        public ReadOnlyReactiveCollection<Person> ListView { get; private set; }
        public ReactiveProperty<Person> SelectedItem { get; private set; } = new ReactiveProperty<Person>();
        public ObservableCollection<int> AgeList { get; private set; } = new ObservableCollection<int>();
        public ObservableCollection<string> GenderList { get; private set; } = new ObservableCollection<string>();

        public ReactiveProperty<int> Age { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<string> Gender { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Name { get; } = new ReactiveProperty<string>();

        public ReactiveCommand InsertTapped { get; private set; } = new ReactiveCommand();
        public ReactiveCommand UpDateTapped { get; private set; } = new ReactiveCommand();
        public ReactiveCommand DeleteTapped { get; private set; } = new ReactiveCommand();

        private Commander commander = new Commander("commander.db3");

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            //PickerList
            AgeList = new ObservableCollection<int>(Enumerable.Range(1, 100).ToList());
            GenderList = new ObservableCollection<string>() { "男", "女", "不明" };

            //ViewModel←Model
            ListView = commander.Commanders.ToReadOnlyReactiveCollection();

            //Button
            InsertTapped.Subscribe(_ => commander.Insert(Age.Value, Name.Value, Gender.Value));
            UpDateTapped.Subscribe(_ => commander.UpDate(SelectedItem.Value, Age.Value, Name.Value, Gender.Value));
            DeleteTapped.Subscribe(_ => commander.Delete(SelectedItem.Value));
        }

    }
}
