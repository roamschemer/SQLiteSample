using Prism.Mvvm;
using SQLite;
using System;
using System.Collections.ObjectModel;

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
        private string dbPath;
        public ObservableCollection<Person> Commanders { get; private set; } = new ObservableCollection<Person>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbPath">SQLite保存名称</param>
        public Commander(string dbPath)
        {
            this.dbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), dbPath);
            DbCreate();
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
        public void UpDate(int id, int age, string name, string gender)
        {
            var person = new Person { Id = id, Age = age, Name = name, Gender = gender };
            DbUpDate(person);
            DbLoad();
        }

        /// <summary>
        /// リストから消す
        /// </summary>
        /// <param name="person">人情報</param>
        public void Delete(int id)
        {
            var person = new Person { Id = id };
            DbDelete(person);
            //再読み込み
            DbLoad();
        }

        /// <summary>
        /// データベース生成
        /// </summary>
        private void DbCreate()
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                db.CreateTable<Person>();
            }
        }

        /// <summary>
        /// データベースへ追加
        /// </summary>
        /// <param name="person">人情報</param>
        private void DbInsert(Person person)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                db.Insert(person);
            }
        }

        /// <summary>
        /// データベースの更新
        /// </summary>
        /// <param name="person">人情報</param>
        private void DbUpDate(Person person)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                db.Update(person);
            }
        }

        /// <summary>
        /// データベースから削除
        /// </summary>
        /// <param name="person">人情報</param>
        private void DbDelete(Person person)
        {
            using (var db = new SQLiteConnection(dbPath))
            {
                db.Delete<Person>(person.Id);
            }
        }

        /// <summary>
        /// データベースの呼出
        /// </summary>
        private void DbLoad()
        {
            Commanders.Clear();
            using (var db = new SQLiteConnection(dbPath))
            {
                //LINQで書きたいが…畜生！わからん！
                foreach (var x in db.Table<Person>())
                {
                    Commanders.Add(x);
                }
            }
        }


    }

}
