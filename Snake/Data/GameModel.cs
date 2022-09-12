using System;
using System.IO;
using System.Collections.Generic;

namespace Snake.Data
{
    // Перечисление направления движения
    public enum MoveDirection { Right, Left, Up, Down } 


    // Класс описывает каждую частичку (клеточку) змейки
    public class BodyElement
    {
        public BodyElement(int col, int row, MoveDirection dir)
        {
            colPosition = col;
            rowPosition = row;
            direction = dir;
        }

        public int colPosition { get; set; } // Положение в пространстве (столбец)
        public int rowPosition { get; set; } // Положение в пространстве (строка)
        public MoveDirection direction { get; set; }
    }


    //  Класс описывает игровые рекорды
    public class Record : IComparable<Record>
    {
        static public List<Record> recordList = new List<Record>(); // Список игровых рекордов
        const string recordPath = @"wwwroot\GameRecords.txt"; // путь к текстовому файлу рекордов
        static public string recordErrorMessage; // Сообщение об ошибке при чтении файла рекордов
        const int sizeRecordList = 10; // Размер списка рекордов

        public string Name { get; set; }
        public int Score { get; set; }

        public Record(string name, int score)
        {
            Name = name;
            Score = score;
        }

        // Чтение файла рекордов
        static public void ReadFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(recordPath))
                {
                    recordList = new List<Record>();
                    string recordLine;
                    string[] recordSplit;
                    while ((recordLine = sr.ReadLine()) != null)
                    {
                        recordSplit = recordLine.Split(' ');
                        Record record = new Record(recordSplit[0], Convert.ToInt32(recordSplit[1]));
                        recordList.Add(record);
                    }
                }
                recordList.Sort();
                recordErrorMessage = null;
            }
            catch
            {
                recordErrorMessage = "Неверная структура файла!";
            }
        }


        // Сортировка по убыванию
        public int CompareTo(Record obj) 
        {
            if (this.Score == obj.Score) return 0;
            else if (this.Score > obj.Score) return -1;
            else return 1;
        }

        // Добавление нового рекорда
        public static void AddNewRecord(string name, int score, int position)
        {
            Record newRecord = new Record(name, score);
            recordList.Insert(position, newRecord);
            if (recordList.Count == 11) recordList.RemoveAt(10);
            // Полностью переписываем файл, чтобы сохранить численность и порядок рекордов
            using (StreamWriter sw = new StreamWriter(recordPath, false))
            {
                for (int i = 0; i < recordList.Count; i++)
                {
                    sw.Write(recordList[i].Name);
                    sw.Write(" ");
                    sw.WriteLine(recordList[i].Score);
                }
            }
        }

        // Проверка на новый рекорд, возврат - позиция в списке рекордов
        public static int CheckNewRecord(int score)
        {
            if (recordList.Count == 0) // Если список пустой
            {
                return 0;
            }
            else
            {
                for (int i = 0; i < recordList.Count; i++)
                {
                    if (score <= recordList[i].Score)
                    {
                        continue;
                    }
                    else
                    {
                        return i;
                    }
                }
            }
            if (recordList.Count < sizeRecordList) // Если список не полный
            {
                return recordList.Count;
            }
            return -1;
        }
    }
}
