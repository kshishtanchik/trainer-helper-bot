using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseService.Models;
using DatabaseService.Repositories;
using DatabaseService.Services;
using Moq;
using NUnit.Framework;

namespace DatabaseService.Tests
{
    [TestFixture]
    public class TrainingServiceTests
    {
        private Mock<ITrainingRepository> _mockRepository;
        private TrainingService _trainingService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ITrainingRepository>();
            _trainingService = new TrainingService(_mockRepository.Object);
        }

        [Test]
        public async Task GetTrainingAsync_ReturnsTraining()
        {
            var training = new Activity { Id = 1, Name = "Yoga", Date = DateTime.Now, Facilitator = 1 };
            _mockRepository.Setup(repo => repo.GetTrainingAsync(1)).ReturnsAsync(training);

            var result = await _trainingService.GetTrainingAsync(1);

            Assert.That(result != null);
            Assert.Equals(training.Id, result.Id);
        }

        [Test]
        public async Task GetAllTrainingsAsync_ReturnsAllTrainings()
        {
            var trainings = new List<Activity>
            {
                new Activity { Id = 1, Name = "Yoga", Date = DateTime.Now, Facilitator = 1 },
                new Activity { Id = 2, Name = "Pilates", Date = DateTime.Now, Facilitator = 2 }
            };
            _mockRepository.Setup(repo => repo.GetAllTrainingsAsync()).ReturnsAsync(trainings);

            var result = await _trainingService.GetAllTrainingsAsync();

            Assert.That(result !=null);
            Assert.Equals(2, result.Count());
        }

        [Test]
        public async Task AddTrainingAsync_AddsTraining()
        {
            var training = new Activity { Id = 1, Name = "Yoga", Date = DateTime.Now, Facilitator = 1 };
            _mockRepository.Setup(repo => repo.AddTrainingAsync(training)).Returns(Task.CompletedTask);

            await _trainingService.AddTrainingAsync(training);

            _mockRepository.Verify(repo => repo.AddTrainingAsync(training), Times.Once);
        }

        [Test]
        public async Task UpdateTrainingAsync_UpdatesTraining()
        {
            var training = new Activity { Id = 1, Name = "Yoga", Date = DateTime.Now, Facilitator = 1 };
            _mockRepository.Setup(repo => repo.UpdateTrainingAsync(training)).Returns(Task.CompletedTask);

            await _trainingService.UpdateTrainingAsync(training);

            _mockRepository.Verify(repo => repo.UpdateTrainingAsync(training), Times.Once);
        }

        [Test]
        public async Task DeleteTrainingAsync_DeletesTraining()
        {
            _mockRepository.Setup(repo => repo.DeleteTrainingAsync(1)).Returns(Task.CompletedTask);

            await _trainingService.DeleteTrainingAsync(1);

            _mockRepository.Verify(repo => repo.DeleteTrainingAsync(1), Times.Once);
        }
    }
}