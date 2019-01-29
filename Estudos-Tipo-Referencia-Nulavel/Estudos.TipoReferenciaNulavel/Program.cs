using System;
using System.Collections.Generic;
using System.Linq;

namespace Estudos.TipoReferenciaNulavel
{
    public enum QuestionType
    {
        YesNo,
        Number,
        Text
    }

    public class SurveyQuestion
    {
        public string QuestionText { get; }
        public QuestionType TypeOfQuestion { get; }
        
        public SurveyQuestion(QuestionType typeOfQuestion, string text) =>
            (TypeOfQuestion, QuestionText) = (typeOfQuestion, text);
    }
    
    public class SurveyRun
    {
        private List<SurveyQuestion> surveyQuestions = new List<SurveyQuestion>();

        public void AddQuestion(QuestionType type, string question) =>
            AddQuestion(new SurveyQuestion(type, question));
        public void AddQuestion(SurveyQuestion surveyQuestion) => surveyQuestions.Add(surveyQuestion);
        
        private List<SurveyResponse>? respondents;
        public void PerformSurvey(int numberOfRespondents)
        {
            int repondentsConsenting = 0;
            respondents = new List<SurveyResponse>();
            while (repondentsConsenting < numberOfRespondents)
            {
                var respondent = SurveyResponse.GetRandomId();
                if (respondent.AnswerSurvey(surveyQuestions))
                    repondentsConsenting++;
                respondents.Add(respondent);
            }
        }
        
        public IEnumerable<SurveyResponse> AllParticipants => (respondents ?? Enumerable.Empty<SurveyResponse>());
        public ICollection<SurveyQuestion> Questions => surveyQuestions;
        public SurveyQuestion GetQuestion(int index) => surveyQuestions[index];
        
        // public bool AnsweredSurvey => surveyResponses != null;
        // public string Answer(int index) => surveyResponses?.GetValueOrDefault(index) ?? "No answer";
    }
    
    public class SurveyResponse
    {
        public int Id { get; }

        public SurveyResponse(int id) => Id = id;
        
        private static readonly Random randomGenerator = new Random();
        public static SurveyResponse GetRandomId() => new SurveyResponse(randomGenerator.Next());
        
        private Dictionary<int, string>? surveyResponses;
        public bool AnswerSurvey(IEnumerable<SurveyQuestion> questions)
        {
            if (ConsentToSurvey())
            {
                surveyResponses = new Dictionary<int, string>();
                int index = 0;
                foreach (var question in questions)
                {
                    var answer = GenerateAnswer(question);
                    if (answer != null)
                    {
                        surveyResponses.Add(index, answer);
                    }
                    index++;
                }
            }
            return surveyResponses != null;
        }

        private bool ConsentToSurvey() => randomGenerator.Next(0, 2) == 1;

        private string? GenerateAnswer(SurveyQuestion question)
        {
            switch (question.TypeOfQuestion)
            {
                case QuestionType.YesNo:
                    int n = randomGenerator.Next(-1, 2);
                    return (n == -1) ? default : (n == 0) ? "No" : "Yes";
                case QuestionType.Number:
                    n = randomGenerator.Next(-30, 101);
                    return (n < 0) ? default : n.ToString();
                case QuestionType.Text:
                default:
                    switch (randomGenerator.Next(0, 5))
                    {
                        case 0:
                            return default;
                        case 1:
                            return "Red";
                        case 2:
                            return "Green";
                        case 3:
                            return "Blue";
                    }
                    return "Red. No, Green. Wait.. Blue... AAARGGGGGHHH!";
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var surveyRun = new SurveyRun();
            surveyRun.AddQuestion(QuestionType.YesNo, "Has your code ever thrown a NullReferenceException?");
            surveyRun.AddQuestion(new SurveyQuestion(QuestionType.Number, "How many times (to the nearest 100) has that happened?"));
            surveyRun.AddQuestion(QuestionType.Text, "What is your favorite color?");
            surveyRun.AddQuestion(QuestionType.Text, default);
            
            surveyRun.PerformSurvey(50);
            
            // foreach (var participant in surveyRun.AllParticipants)
            // {
            //     Console.WriteLine($"Participant: {participant.Id}:");
            //     if (participant.AnsweredSurvey)
            //     {
            //         for (int i = 0; i < surveyRun.Questions.Count; i++)
            //         {
            //             var answer = participant.Answer(i);
            //             Console.WriteLine($"\t{surveyRun.GetQuestion(i)} : {answer}");
            //         }
            //     }
            //     else
            //         Console.WriteLine("\tNo responses");
            // }
        }
    }
}