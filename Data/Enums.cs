using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PGC.Data {
    public enum OrderType {
        Зарахування = 1,
        Переведення = 2,
        Закінчення = 3,
        Відрахування = 4,
        Академвідпустка = 5,

        [Display (Name = "Повернення з відпустки")]
        Вихід_з_відпустки = 6,

        [Display (Name = "Зміна форми навчання")]
        Зміна_форми_навчання = 7,

        Інший = 8,

    }

    public enum StatusType : int {
        новий = 1,
        навчання = 2,

        [Display (Name = "акд.відп")]
        академ_відпустка = 3,

        [Display (Name = "відрахов.")]
        відрахований = 4,

        [Display (Name = "зах.достр")]
        захист_достроково = 5,
        закінчив = 6,

        [Display (Name = "пвт.курс")]
        повторний_курс = 7,

        [Display (Name = "стаж.")]
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
        доцент = 2,

        [Display (Name = "старший дослідник")]
        ст_дослідник = 3
    }

    public enum Degree {
        [Display (Name = "-")]
        відс = 0,

        [Display (Name = "доктор наук")]
        доктор_наук = 1,

        [Display (Name = "кандидат наук")]
        кандидат_наук = 2,

        [Display (Name = "PhD")]
        доктор_філософії = 3

    }

    public enum Position : int {
        [Display (Name = "-")]
        відс = 0,

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
        public string Сomment { get; set; }
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