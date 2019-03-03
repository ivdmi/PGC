using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PGC.Data {
    public enum OrderTypeEnum {
        Зарахування = 1,
        Переведення = 2,
        Закінчення = 3,
        Відрахування = 4,
        Академвідпустка = 5,
        Інший = 6,
        Зміна_форми_навчання = 7
    }

    public enum StatusTypeEnum : int {
        новий = 1,
        навчання = 2, [Display (Name = "академ.відпустка")]
        академ_відпустка = 3,
        відрахований_2 = 4, [Display (Name = "захист достроково")]
        захист_достроково = 5,
        закінчив = 6, [Display (Name = "повтор.курс")]
        повторний_курс = 7,
        стажування = 8
    }

    public enum StudyForm : int {
        денна = 1,
        вечірня = 2,
        заочна = 3
    }

    public enum Rank : int {
        [Display (Name = "-")]
        відс = 0,
        професор = 1,
        доцент = 2, [Display (Name = "старший дослідник")]
        ст_дослідник = 3
    }

    public enum Degree {
        [Display (Name = "-")]
        відс = 0, [Display (Name = "доктор наук")]
        доктор_наук = 1, [Display (Name = "кандидат наук")]
        кандидат_наук = 2, [Display (Name = "доктор філософії")]
        доктор_філософії = 3

    }

    public enum Position : int {
        [Display (Description = "декан факультету")]
        декан = 1,

        [Display (Description = "директор інституту")]
        директор = 2,

        [Display (Name = "заст.декана", Description = "заступник декана факультету")]
        заст_декана = 3,

        [Display (Name = "заст.директора", Description = "заступник директора інституту")]
        заст_директора = 4,

        [Display (Name = "зав.каф.")]
        зав_каф = 5,

        [Display (Name = "професор")]
        професор = 6,

        [Display (Name = "доцент")]
        доцент = 7,

        [Display (Name = "ст.викладач", Description = "старший викладач")]
        ст_викладач = 8,

        [Display (Name = "викладач", Description = "викладач")]
        викладач = 9,

        [Display (Name = "інше", Description = "тут може бути ваша реклама")]
        інше = 8
    }

    public class ItemData {
        public int Value { get; set; }
        public string Text { get; set; }
    }
    public static class Helper {
        // public static IList<ItemData> GetPositions () {
        //     IList<ItemData> list = new List<ItemData> ();
        //     foreach (Position item in Enum.GetValues (typeof (Position))) {
        //         list.Add (item: new ItemData () { Value = (int) item, Text = item.ToString () });
        //     }
        //     return list;
        // }
        public static IList<ItemData> GetDegrees () {
            IList<ItemData> list = new List<ItemData> ();
            foreach (Degree item in Enum.GetValues (typeof (Degree))) {
                list.Add (item: new ItemData () { Value = (int) item, Text = item.GetDisplayName () });
            }
            return list;
        }

        public static IList<ItemData> GetRanks () {
            IList<ItemData> list = new List<ItemData> ();
            foreach (Rank item in Enum.GetValues (typeof (Rank))) {
                list.Add (item: new ItemData () { Value = (int) item, Text = item.GetDisplayName () });
            }
            return list;
        }

        public static IList<ItemData> GetPositions () {
            IList<ItemData> list = new List<ItemData> ();
            foreach (Position item in Enum.GetValues (typeof (Position))) {
                list.Add (item: new ItemData () { Value = (int) item, Text = item.GetDisplayName () });
            }
            return list;
        }

        public static IList<ItemData> GetStudyForms () {
            IList<ItemData> list = new List<ItemData> ();
            foreach (StudyForm item in Enum.GetValues (typeof (StudyForm))) {
                list.Add (item: new ItemData () { Value = (int) item, Text = item.GetDisplayName () });
            }
            return list;
        }

        public static IList<ItemData> GetStatusList () {
            IList<ItemData> list = new List<ItemData> ();
            foreach (StatusTypeEnum item in Enum.GetValues (typeof (StatusTypeEnum))) {
                list.Add (item: new ItemData () { Value = (int) item, Text = item.GetDisplayName () });
            }
            return list;
        }

        public static IList<ItemData> GetOrderTypeList () {
            IList<ItemData> list = new List<ItemData> ();
            foreach (OrderTypeEnum item in Enum.GetValues (typeof (OrderTypeEnum))) {
                list.Add (item: new ItemData () { Value = (int) item, Text = item.GetDisplayName () });
            }
            return list;
        }

    }
}

// Расширяющий Enum класс - для расширяющих методов получения атрибутов из Enum
public static class GetExtentions {
    public static string GetDisplayName (this Enum enu) {
        var attr = GetDisplayAttribute (enu);
        bool result = (attr != null && !String.IsNullOrEmpty (attr.Name));
        return result ? attr.Name : enu.ToString ();
    }

    public static string GetDescription (this Enum enu) {
        var attr = GetDisplayAttribute (enu);
        bool result = (attr != null && !String.IsNullOrEmpty (attr.Description));
        return result ? attr.Description : enu.ToString ();
    }

    private static DisplayAttribute GetDisplayAttribute (object value) {
        Type type = value.GetType ();
        if (!type.IsEnum) {
            throw new ArgumentException (string.Format ("Type {0} is not an enum", type));
        }
        // Get enum field
        var field = type.GetField (value.ToString ());
        var atr = field.GetCustomAttributes (typeof (DisplayAttribute), false);
        return field == null ? null : field.GetCustomAttribute<DisplayAttribute> ();
    }
}