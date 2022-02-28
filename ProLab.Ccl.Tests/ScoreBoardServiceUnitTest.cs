using AutoFixture;
using MediatR;
using Moq;
using NUnit.Framework;
using ProLab.Ccl.ApplicationServices.Features.ScoreBoard.Queries;
using ProLab.Ccl.Domain.Models;
using ProLab.Ccl.DomainServices.RepositoryInterfaces;
using System;
using System.Collections.Generic;

namespace ProLab.Ccl.Tests
{
    public class ScoreBoardServiceUnitTest
    {

        private readonly Mock<IScoreBoardRepository> service;
        public ScoreBoardServiceUnitTest()
        {
            service = new Mock<IScoreBoardRepository>();
        }
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Create_Author_Should_Call_Add_Method_Once()
        {
            var fixture = new Fixture();
            var command = fixture.Create<GetScoreBoardDetailByIdentityIdAndTypeIdQuery>();
            //var mockUnitOfWork = new Mock<IUnitOfWork>();
            //var mockHandler = new Mock<IRequestHandler<GetScoreBoardDetailByIdentityIdAndTypeIdQuery, ApiResponse>>();

            //var result = mockHandler.Object.Handle(command);

            //mockUnitOfWork.Verify(x => x.AuthorRepository.AddAsync(It.IsAny<Author>()), Times.Once);
        }




        private List<ScoreBoardDetail> GetSampleData()
        {
            List<ScoreBoardDetail> output = new List<ScoreBoardDetail>
        {
            new ScoreBoardDetail
            {
                Id = 1,
                IdentityId = 1,
                Value = 26167,
                EntryDate = DateTime.Now,
                 ScoreBoardDetailTypeEnum = Domain.Enums.ScoreBoardDetailTypeEnum.IncreaseByLogin,
            },
            new ScoreBoardDetail
            {
                Id = 2,
                IdentityId = 3,
                Value = 87665,
                EntryDate = DateTime.Now,
                 ScoreBoardDetailTypeEnum = Domain.Enums.ScoreBoardDetailTypeEnum.IncreaseByLogin,
            },
            new ScoreBoardDetail
            {
                Id = 3,
                IdentityId = 2,
                Value = 343567,
                EntryDate = DateTime.Now,
                 ScoreBoardDetailTypeEnum = Domain.Enums.ScoreBoardDetailTypeEnum.IncreaseByLogin,
            },
            new ScoreBoardDetail
            {
                Id = 4,
                IdentityId = 3,
                Value = 97667,
                EntryDate = DateTime.Now,
                 ScoreBoardDetailTypeEnum = Domain.Enums.ScoreBoardDetailTypeEnum.IncreaseByLogin,
            },
            new ScoreBoardDetail
            {
                Id = 5,
                IdentityId = 1,
                Value = 4356,
                EntryDate = DateTime.Now,
                 ScoreBoardDetailTypeEnum = Domain.Enums.ScoreBoardDetailTypeEnum.IncreaseByLogin,
            }
        };
            return output;
        }
    }
}