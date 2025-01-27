Imports System.IO
Imports System.Text.Json

Module Program
    Sub Main()
        Dim flashCardApp As New FlashCardApp()
        flashCardApp.Run()
    End Sub
End Module

Public Class FlashCard
    Public Property Question As String
    Public Property Answer As String
    Public Property LastReviewDate As DateTime?
    Public Property TimesReviewed As Integer

    Public Sub New(question As String, answer As String)
        Me.Question = question
        Me.Answer = answer
        Me.TimesReviewed = 0
    End Sub
End Class

Public Class FlashCardApp
    Private ReadOnly cards As List(Of FlashCard)
    Private ReadOnly fileName As String = "flashcards.json"

    Public Sub New()
        cards = New List(Of FlashCard)()
        LoadCards()
    End Sub

    Public Sub Run()
        Dim running As Boolean = True

        While running
            Console.Clear()
            Console.WriteLine("=== Flash Card Application ===")
            Console.WriteLine("1. Add new card")
            Console.WriteLine("2. View all cards")
            Console.WriteLine("3. Review cards")
            Console.WriteLine("4. Save cards")
            Console.WriteLine("5. Load cards")
            Console.WriteLine("6. Exit")
            Console.WriteLine()
            Console.Write("Select an option (1-6): ")

            Dim choice As String = Console.ReadLine()

            Select Case choice
                Case "1"
                    AddCard()
                Case "2"
                    ViewCards()
                Case "3"
                    ReviewCards()
                Case "4"
                    SaveCards()
                Case "5"
                    LoadCards()
                Case "6"
                    running = False
                Case Else
                    Console.WriteLine("Invalid option. Press any key to continue...")
                    Console.ReadKey()
            End Select
        End While
    End Sub

    Private Sub AddCard()
        Console.Clear()
        Console.WriteLine("=== Add New Card ===")

        Console.Write("Enter question: ")
        Dim question As String = Console.ReadLine()

        Console.Write("Enter answer: ")
        Dim answer As String = Console.ReadLine()

        If Not String.IsNullOrWhiteSpace(question) AndAlso Not String.IsNullOrWhiteSpace(answer) Then
            cards.Add(New FlashCard(question, answer))
            Console.WriteLine("Card added successfully!")
        Else
            Console.WriteLine("Question and answer cannot be empty!")
        End If

        Console.WriteLine("Press any key to continue...")
        Console.ReadKey()
    End Sub

    Private Sub ViewCards()
        Console.Clear()
        Console.WriteLine("=== View All Cards ===")

        If cards.Count = 0 Then
            Console.WriteLine("No cards available.")
        Else
            For i As Integer = 0 To cards.Count - 1
                Console.WriteLine($"Card #{i + 1}")
                Console.WriteLine($"Question: {cards(i).Question}")
                Console.WriteLine($"Answer: {cards(i).Answer}")
                Console.WriteLine($"Times Reviewed: {cards(i).TimesReviewed}")
                Console.WriteLine($"Last Review: {If(cards(i).LastReviewDate, "Never")}")
                Console.WriteLine()
            Next
        End If

        Console.WriteLine("Press any key to continue...")
        Console.ReadKey()
    End Sub

    Private Sub ReviewCards()
        Console.Clear()
        Console.WriteLine("=== Review Cards ===")

        If cards.Count = 0 Then
            Console.WriteLine("No cards available for review.")
            Console.WriteLine("Press any key to continue...")
            Console.ReadKey()
            Return
        End If

        For i As Integer = 0 To cards.Count - 1
            Console.Clear()
            Console.WriteLine($"Card #{i + 1} of {cards.Count}")
            Console.WriteLine($"Question: {cards(i).Question}")
            Console.WriteLine()
            Console.WriteLine("Press Enter to see the answer...")
            Console.ReadLine()

            Console.WriteLine($"Answer: {cards(i).Answer}")
            cards(i).TimesReviewed += 1
            cards(i).LastReviewDate = DateTime.Now

            Console.WriteLine()
            Console.WriteLine("Did you get it right? (Y/N/Q to quit)")
            Dim response As String = Console.ReadLine().ToUpper()

            If response = "Q" Then
                Exit For
            End If
        Next

        Console.WriteLine("Review session completed. Press any key to continue...")
        Console.ReadKey()
    End Sub

    Private Sub SaveCards()
        Try
            Dim jsonString As String = JsonSerializer.Serialize(cards)
            File.WriteAllText(fileName, jsonString)
            Console.WriteLine("Cards saved successfully!")
        Catch ex As Exception
            Console.WriteLine($"Error saving cards: {ex.Message}")
        End Try

        Console.WriteLine("Press any key to continue...")
        Console.ReadKey()
    End Sub

    Private Sub LoadCards()
        Try
            If File.Exists(fileName) Then
                Dim jsonString As String = File.ReadAllText(fileName)
                Dim loadedCards = JsonSerializer.Deserialize(Of List(Of FlashCard))(jsonString)
                cards.Clear()
                cards.AddRange(loadedCards)
                Console.WriteLine("Cards loaded successfully!")
            End If
        Catch ex As Exception
            Console.WriteLine($"Error loading cards: {ex.Message}")
        End Try

        If Not Console.IsInputRedirected Then
            Console.WriteLine("Press any key to continue...")
            Console.ReadKey()
        End If
    End Sub
End Class