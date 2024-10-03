using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Note
{
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }

    public Note(string title, string content)
    {
        Title = title;
        Content = content;
        CreatedAt = DateTime.Now;
    }

    public override string ToString()
    {
        return $"Заголовок: {Title}\nДата создания: {CreatedAt}\nТекст: {Content}";
    }
}

public class NoteManager
{
    private List<Note> Notes { get; set; }

    public NoteManager()
    {
        Notes = new List<Note>();
    }

    // Добавление новой заметки
    public void AddNote(string title, string content)
    {
        Notes.Add(new Note(title, content));
    }

    // Удаление заметки по заголовку
    public bool RemoveNoteByTitle(string title)
    {
        var note = Notes.FirstOrDefault(n => n.Title == title);
        if (note != null)
        {
            Notes.Remove(note);
            return true;
        }
        return false;
    }

    // Редактирование заметки по заголовку
    public bool EditNote(string title, string newContent)
    {
        var note = Notes.FirstOrDefault(n => n.Title == title);
        if (note != null)
        {
            note.Content = newContent;
            return true;
        }
        return false;
    }

    // Сохранение заметок в файл
    public void SaveNotesToFile(string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var note in Notes)
            {
                writer.WriteLine($"{note.Title}|{note.CreatedAt}|{note.Content}");
            }
        }
    }

    // Загрузка заметок из файла
    public void LoadNotesFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            var lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var parts = line.Split('|');
                if (parts.Length == 3)
                {
                    Notes.Add(new Note(parts[0], parts[2])
                    {
                        CreatedAt = DateTime.Parse(parts[1])
                    });
                }
            }
        }
    }

    // Вывод всех заметок
    public void ShowAllNotes()
    {
        foreach (var note in Notes)
        {
            Console.WriteLine(note);
            Console.WriteLine("-------------");
        }
    }
}

class Program
{
    static void Main()
    {
        NoteManager noteManager = new NoteManager();

        noteManager.AddNote("Первая заметка", "Это текст первой заметки.");
        noteManager.AddNote("Вторая заметка", "Это текст второй заметки.");

        noteManager.SaveNotesToFile("notes.txt");

        noteManager.LoadNotesFromFile("notes.txt");

        noteManager.ShowAllNotes();

        noteManager.EditNote("Первая заметка", "Обновленный текст первой заметки.");
        noteManager.RemoveNoteByTitle("Вторая заметка");

        noteManager.SaveNotesToFile("notes_updated.txt");
    }
}