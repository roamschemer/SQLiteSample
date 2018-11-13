using Prism.Navigation;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

        private Commander commander = new Commander("commander");

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            //Pickerリスト作成
            AgeList = new ObservableCollection<int>(Enumerable.Range(1, 100).ToList());
            GenderList = new ObservableCollection<string>() { "男", "女", "不明" };

            //ViewModel←Model
            ListView = commander.Commanders.ToReadOnlyReactiveCollection();

            //Button
            InsertTapped.Subscribe(_ =>
            {
                commander.Insert(Age.Value, Name.Value, Gender.Value);
            });
            UpDateTapped.Subscribe(_ =>
            {
                commander.UpDate(SelectedItem.Value.Id, Age.Value, Name.Value, Gender.Value);
            });
            DeleteTapped.Subscribe(_ =>
            {
                commander.Delete(SelectedItem.Value.Id);
            });
        }

    }
}
