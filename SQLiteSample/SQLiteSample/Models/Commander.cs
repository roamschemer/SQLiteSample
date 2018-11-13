using Prism.Mvvm;
using SQLite;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PCLStorage;

namespace SQLiteSample.Models
{
    public class Person : BindableBase
    {
        //主キー
        [PrimaryKey, AutoIncrement]
        public int Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }
        private int id = 0;

        public int Age
        {
            get => age;
            set => SetProperty(ref age, value);
        }
        private int age = 0;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        private string name = "";

        public string Gender
        {
            get => gender;
            set => SetProperty(ref gender, value);
        }
        private string gender = "";
    }

    public class Commander : BindableBase
    {
        private readonly string dbPath;
        public ObservableCollection<Person> Commanders { get; private set; } = new ObservableCollection<Person>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbPath">SQLite保存名称</param>
        public Commander(string dbPath)
        {
            this.dbPath = dbPath;
            DbLoad();
        }

        /// <summary>
        /// リストに追加
        /// </summary>
        public void Insert(int age, string name, string gender)
        {
            var person = new Person { Age = age, Name = name, Gender = gender };
            DbInsert(person);
            DbLoad();
        }

        /// <summary>
        /// リストの更新
        /// </summary>
        public void UpDate(Person selectedPerson, int age, string name, string gender)
        {
            if (selectedPerson == null) return;
            var person = new Person { Id = selectedPerson.Id, Age = age, Name = name, Gender = gender };
            DbUpDate(person);
            DbLoad();
        }

        /// <summary>
        /// リストから消す
        /// </summary>
        /// <param name="person">人情報</param>
        public void Delete(Person selectedPerson)
        {
            if (selectedPerson == null) return;
            var person = new Person { Id = selectedPerson.Id };
            DbDelete(person);
            DbLoad();
        }

        /// <summary>
        /// データベースへ追加
        /// </summary>
        /// <param name="person">人情報</param>
        private async void DbInsert(Person person)
        {
            using (var db = await CreateDb())
            {
                db.Insert(person);
            }
        }

        /// <summary>
        /// データベースの更新
        /// </summary>
        /// <param name="person">人情報</param>
        private async void DbUpDate(Person person)
        {
            using (var db = await CreateDb())
            {
                db.Update(person);
            }
        }

        /// <summary>
        /// データベースから削除
        /// </summary>
        /// <param name="person">人情報</param>
        private async void DbDelete(Person person)
        {
            using (var db = await CreateDb())
            {
                db.Delete<Person>(person.Id);
            }
        }

        /// <summary>
        /// データベースの呼出
        /// </summary>
        private async void DbLoad()
        {
            Commanders.Clear();
            using (var db = await CreateDb())
            {
                foreach (var x in db.Table<Person>())
                {
                    Commanders.Add(x);
                }
            }
        }

        /// <summary>
        /// データベースの生成と取得(以下のようにPCLStorageを使わないとUWPで例外が発生した)
        /// </summary>
        /// <returns></returns>
        private async Task<SQLiteConnection> CreateDb()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            var result = await rootFolder.CheckExistsAsync(dbPath);
            if (result == ExistenceCheckResult.NotFound)
            {
                IFile file = await rootFolder.CreateFileAsync(dbPath, CreationCollisionOption.ReplaceExisting);
                var db = new SQLiteConnection(file.Path);
                db.CreateTable<Person>();
                return db;
            }
            else
            {
                IFile file = await rootFolder.CreateFileAsync(dbPath, CreationCollisionOption.OpenIfExists);
                return new SQLiteConnection(file.Path);
            }
        }

    }

}
