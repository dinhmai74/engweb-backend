using HollypocketBackend.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HollypocketBackend.Utils;
using AutoMapper;

namespace HollypocketBackend.Services
{
    public class QuestionService
    {
        private readonly IMongoCollection<Question> _questions;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        public QuestionService(AppSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _mapper = mapper;
            _appSettings = settings;
            _questions = database.GetCollection<Question>(settings.QuestionCollectionName);
        }
        public async Task<PagedList<Question>> GetWithPage(string productId, int pageSize, int pageNumber)
        {
            return await PagedList<Question>.ToPagedList(_questions.Find(b => b.productId == productId).ToList(), pageSize, pageNumber);
        }
        public List<Question> Get() =>
        _questions.Find(question => true).ToList();


        public Question Create(QuestionDto question, string userId)
        {
            var _question = _mapper.Map<Question>(question);
            _question.userId = userId;
            _questions.InsertOne(_question);
            return _question;
        }

        public Question AnswerQuestion(string answer, string questionId)
        {
            var question = Get(questionId);
            question.Answer = answer;
            _questions.ReplaceOneAsync(q=> q.Id==questionId, question);
            return question;
        }
        public Question Get(string id) =>
        _questions.Find<Question>(b => b.Id == id).FirstOrDefault();

        public void Update(string id, Question questionIn) =>
        _questions.ReplaceOne(question => question.Id == id, questionIn);
        
        public void Delete(Question questionIn) => _questions.DeleteOne(b => b.Id == questionIn.Id);
        public void Delete(string id) => _questions.DeleteOne(b => b.Id == id);
    }
}
