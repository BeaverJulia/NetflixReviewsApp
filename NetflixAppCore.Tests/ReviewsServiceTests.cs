using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NetflixReviewsApp.core;
using NetflixReviewsApp.core.Models;
using NetflixReviewsApp.core.Services;
using NetflixReviewsApp.data;
using NetflixReviewsApp.data.Entities;
using NUnit.Framework;
using RestSharp;

namespace NetflixAppCore.Tests
{
    //TODO Add Integration tests.
    public class ReviewServiceTests
    {
        private readonly DataContext _context;
        public Mock<IOpenWrksApiService> OpenWrksApiService;
        private IReviewsService _sut;
        private static IMapper _mapper;

        public ReviewServiceTests()
        {
            var contextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new DataContext(contextOptions);
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
                var mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }


        [SetUp]
        public void Setup()
        {
            OpenWrksApiService = new Mock<IOpenWrksApiService>();
            _sut = new ReviewsService(OpenWrksApiService.Object, _context, _mapper);
        }

        [Test]
        public void AddReview_ShouldReturn_NotSuccess_WhenShowNotFound()
        {
            // Arrange
            var review = new ReviewInput
            {
                Description = "test description",
                Nickname = "test nickname",
                ShowId = "s",
                Stars = 1
            };
            IRestResponse response = new RestResponse
            {
                StatusCode = HttpStatusCode.NotFound
            };
            //Act
            OpenWrksApiService.Setup(x => x.GetShow(It.IsAny<string>())).ReturnsAsync(response);
            var act = _sut.AddReview(review);
            //Assert
            Assert.IsFalse(act.Result.Success);
        }

        [Test]
        public void AddReview_ShouldReturn_Success_WhenShowFound()
        {
            // Arrange
            var review = new ReviewInput
            {
                Description = "test description",
                Nickname = "test nickname",
                ShowId = "s1",
                Stars = 1
            };
            IRestResponse response = new RestResponse
            {
                StatusCode = HttpStatusCode.OK
            };
            //Act
            OpenWrksApiService.Setup(x => x.GetShow(It.IsAny<string>())).ReturnsAsync(response);
            var act = _sut.AddReview(review);
            //Assert
            Assert.IsTrue(act.Result.Success);
        }

        [Test]
        public void GetShows_ShouldReturn_Null_WhenExternalApiRequest_NotSucceded()
        {
            // Arrange

            IRestResponse response = new RestResponse
            {
                StatusCode = HttpStatusCode.NotFound
            };
            var pagination = new PaginationFilter
            {
                Limit = 20,
                PageNumber = 1
            };
            //Act
            OpenWrksApiService.Setup(x => x.GetShows(pagination)).ReturnsAsync(response);
            var act = _sut.GetShowsWithReviews(pagination);
            //Assert
            Assert.IsNull(act.Result);
        }

        [Test]
        public void GetShows_ShouldReturnListofShows_WhenExternalApiRequestSucceded()
        {
            // Arrange
            var examples = new List<ReviewEntity>
            {
                new ReviewEntity
                {
                    Id = "1",
                    Description = "test description",
                    Nickname = "test nickname",
                    ShowId = "s1",
                    Stars = 1
                },
                new ReviewEntity
                {
                    Id = "2",
                    Description = "test description",
                    Nickname = "test nickname",
                    ShowId = "s13",
                    Stars = 1
                }
            };
            IRestResponse response = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content= "{\"data\":[{\"id\":\"s1\",\"type\":\"TV Show\",\"title\":\"3%\",\"director\":\"\",\"cast\":\"João Miguel, Bianca Comparato, Michel Gomes, Rodolfo Valente, Vaneza Oliveira, Rafael Lozano, Viviane Porto, Mel Fronckowiak, Sergio Mamberti, Zezé Motta, Celso Frateschi\",\"country\":\"Brazil\",\"dateAdded\":\"2020-08-14T00:00:00\",\"releaseYear\":2020,\"rating\":\"TV-MA\",\"duration\":\"4 Seasons\",\"listedIn\":\"International TV Shows, TV Dramas, TV Sci-Fi & Fantasy\",\"description\":\"In a future where the elite inhabit an island paradise far from the crowded slums, you get one chance to join the 3% saved from squalor.\"},{\"id\":\"s2\",\"type\":\"Movie\",\"title\":\"7:19\",\"director\":\"Jorge Michel Grau\",\"cast\":\"Demián Bichir, Héctor Bonilla, Oscar Serrano, Azalia Ortiz, Octavio Michel, Carmen Beato\",\"country\":\"Mexico\",\"dateAdded\":\"2016-12-23T00:00:00\",\"releaseYear\":2016,\"rating\":\"TV-MA\",\"duration\":\"93 min\",\"listedIn\":\"Dramas, International Movies\",\"description\":\"After a devastating earthquake hits Mexico City, trapped survivors from all walks of life wait to be rescued while trying desperately to stay alive.\"},{\"id\":\"s3\",\"type\":\"Movie\",\"title\":\"23:59\",\"director\":\"Gilbert Chan\",\"cast\":\"Tedd Chan, Stella Chung, Henley Hii, Lawrence Koh, Tommy Kuan, Josh Lai, Mark Lee, Susan Leong, Benjamin Lim\",\"country\":\"Singapore\",\"dateAdded\":\"2018-12-20T00:00:00\",\"releaseYear\":2011,\"rating\":\"R\",\"duration\":\"78 min\",\"listedIn\":\"Horror Movies, International Movies\",\"description\":\"When an army recruit is found dead, his fellow soldiers are forced to confront a terrifying secret that's haunting their jungle island training camp.\"},{\"id\":\"s4\",\"type\":\"Movie\",\"title\":\"9\",\"director\":\"Shane Acker\",\"cast\":\"Elijah Wood, John C. Reilly, Jennifer Connelly, Christopher Plummer, Crispin Glover, Martin Landau, Fred Tatasciore, Alan Oppenheimer, Tom Kane\",\"country\":\"United States\",\"dateAdded\":\"2017-11-16T00:00:00\",\"releaseYear\":2009,\"rating\":\"PG-13\",\"duration\":\"80 min\",\"listedIn\":\"Action & Adventure, Independent Movies, Sci-Fi & Fantasy\",\"description\":\"In a postapocalyptic world, rag-doll robots hide in fear from dangerous machines out to exterminate them, until a brave newcomer joins the group.\"},{\"id\":\"s5\",\"type\":\"Movie\",\"title\":\"21\",\"director\":\"Robert Luketic\",\"cast\":\"Jim Sturgess, Kevin Spacey, Kate Bosworth, Aaron Yoo, Liza Lapira, Jacob Pitts, Laurence Fishburne, Jack McGee, Josh Gad, Sam Golzari, Helen Carey, Jack Gilpin\",\"country\":\"United States\",\"dateAdded\":\"2020-01-01T00:00:00\",\"releaseYear\":2008,\"rating\":\"PG-13\",\"duration\":\"123 min\",\"listedIn\":\"Dramas\",\"description\":\"A brilliant group of students become card-counting experts with the intent of swindling millions out of Las Vegas casinos by playing blackjack.\"},{\"id\":\"s6\",\"type\":\"TV Show\",\"title\":\"46\",\"director\":\"Serdar Akar\",\"cast\":\"Erdal Beşikçioğlu, Yasemin Allen, Melis Birkan, Saygın Soysal, Berkan Şal, Metin Belgin, Ayça Eren, Selin Uludoğan, Özay Fecht, Suna Yıldızoğlu\",\"country\":\"Turkey\",\"dateAdded\":\"2017-07-01T00:00:00\",\"releaseYear\":2016,\"rating\":\"TV-MA\",\"duration\":\"1 Season\",\"listedIn\":\"International TV Shows, TV Dramas, TV Mysteries\",\"description\":\"A genetics professor experiments with a treatment for his comatose sister that blends medical and shamanic cures, but unlocks a shocking side effect.\"},{\"id\":\"s7\",\"type\":\"Movie\",\"title\":\"122\",\"director\":\"Yasir Al Yasiri\",\"cast\":\"Amina Khalil, Ahmed Dawood, Tarek Lotfy, Ahmed El Fishawy, Mahmoud Hijazi, Jihane Khalil, Asmaa Galal, Tara Emad\",\"country\":\"Egypt\",\"dateAdded\":\"2020-06-01T00:00:00\",\"releaseYear\":2019,\"rating\":\"TV-MA\",\"duration\":\"95 min\",\"listedIn\":\"Horror Movies, International Movies\",\"description\":\"After an awful accident, a couple admitted to a grisly hospital are separated and must find each other to escape — before death finds them.\"},{\"id\":\"s8\",\"type\":\"Movie\",\"title\":\"187\",\"director\":\"Kevin Reynolds\",\"cast\":\"Samuel L. Jackson, John Heard, Kelly Rowan, Clifton Collins Jr., Tony Plana\",\"country\":\"United States\",\"dateAdded\":\"2019-11-01T00:00:00\",\"releaseYear\":1997,\"rating\":\"R\",\"duration\":\"119 min\",\"listedIn\":\"Dramas\",\"description\":\"After one of his high school students attacks him, dedicated teacher Trevor Garfield grows weary of the gang warfare in the New York City school system and moves to California to teach there, thinking it must be a less hostile environment.\"},{\"id\":\"s9\",\"type\":\"Movie\",\"title\":\"706\",\"director\":\"Shravan Kumar\",\"cast\":\"Divya Dutta, Atul Kulkarni, Mohan Agashe, Anupam Shyam, Raayo S. Bakhirta, Yashvit Sancheti, Greeva Kansara, Archan Trivedi, Rajiv Pathak\",\"country\":\"India\",\"dateAdded\":\"2019-04-01T00:00:00\",\"releaseYear\":2019,\"rating\":\"TV-14\",\"duration\":\"118 min\",\"listedIn\":\"Horror Movies, International Movies\",\"description\":\"When a doctor goes missing, his psychiatrist wife treats the bizarre medical condition of a psychic patient, who knows much more than he's leading on.\"},{\"id\":\"s10\",\"type\":\"Movie\",\"title\":\"1920\",\"director\":\"Vikram Bhatt\",\"cast\":\"Rajneesh Duggal, Adah Sharma, Indraneil Sengupta, Anjori Alagh, Rajendranath Zutshi, Vipin Sharma, Amin Hajee, Shri Vallabh Vyas\",\"country\":\"India\",\"dateAdded\":\"2017-12-15T00:00:00\",\"releaseYear\":2008,\"rating\":\"TV-MA\",\"duration\":\"143 min\",\"listedIn\":\"Horror Movies, International Movies, Thrillers\",\"description\":\"An architect and his wife move into a castle that is slated to become a luxury hotel. But something inside is determined to stop the renovation.\"},{\"id\":\"s11\",\"type\":\"Movie\",\"title\":\"1922\",\"director\":\"Zak Hilditch\",\"cast\":\"Thomas Jane, Molly Parker, Dylan Schmid, Kaitlyn Bernard, Bob Frazer, Brian d'Arcy James, Neal McDonough\",\"country\":\"United States\",\"dateAdded\":\"2017-10-20T00:00:00\",\"releaseYear\":2017,\"rating\":\"TV-MA\",\"duration\":\"103 min\",\"listedIn\":\"Dramas, Thrillers\",\"description\":\"A farmer pens a confession admitting to his wife's murder, but her death is just the beginning of a macabre tale. Based on Stephen King's novella.\"},{\"id\":\"s12\",\"type\":\"TV Show\",\"title\":\"1983\",\"director\":\"\",\"cast\":\"Robert Więckiewicz, Maciej Musiał, Michalina Olszańska, Andrzej Chyra, Clive Russell, Zofia Wichłacz, Edyta Olszówka, Mateusz Kościukiewicz, Ewa Błaszczyk, Vu Le Hong, Tomasz Włosok, Krzysztof Wach\",\"country\":\"Poland, United States\",\"dateAdded\":\"2018-11-30T00:00:00\",\"releaseYear\":2018,\"rating\":\"TV-MA\",\"duration\":\"1 Season\",\"listedIn\":\"Crime TV Shows, International TV Shows, TV Dramas\",\"description\":\"In this dark alt-history thriller, a naïve law student and a world-weary detective uncover a conspiracy that has tyrannized Poland for decades.\"},{\"id\":\"s13\",\"type\":\"TV Show\",\"title\":\"1994\",\"director\":\"Diego Enrique Osorno\",\"cast\":\"\",\"country\":\"Mexico\",\"dateAdded\":\"2019-05-17T00:00:00\",\"releaseYear\":2019,\"rating\":\"TV-MA\",\"duration\":\"1 Season\",\"listedIn\":\"Crime TV Shows, Docuseries, International TV Shows\",\"description\":\"Archival video and new interviews examine Mexican politics in 1994, a year marked by the rise of the EZLN and the assassination of Luis Donaldo Colosio.\"},{\"id\":\"s14\",\"type\":\"Movie\",\"title\":\"2,215\",\"director\":\"Nottapon Boonprakob\",\"cast\":\"Artiwara Kongmalai\",\"country\":\"Thailand\",\"dateAdded\":\"2019-03-01T00:00:00\",\"releaseYear\":2018,\"rating\":\"TV-MA\",\"duration\":\"89 min\",\"listedIn\":\"Documentaries, International Movies, Sports Movies\",\"description\":\"This intimate documentary follows rock star Artiwara Kongmalai on his historic, 2,215-kilometer charity run across Thailand in 2017.\"},{\"id\":\"s15\",\"type\":\"Movie\",\"title\":\"3022\",\"director\":\"John Suits\",\"cast\":\"Omar Epps, Kate Walsh, Miranda Cosgrove, Angus Macfadyen, Jorja Fox, Enver Gjokaj, Haaz Sleiman\",\"country\":\"United States\",\"dateAdded\":\"2020-03-19T00:00:00\",\"releaseYear\":2019,\"rating\":\"R\",\"duration\":\"91 min\",\"listedIn\":\"Independent Movies, Sci-Fi & Fantasy, Thrillers\",\"description\":\"Stranded when the Earth is suddenly destroyed in a mysterious cataclysm, the astronauts aboard a marooned space station slowly lose their minds.\"},{\"id\":\"s16\",\"type\":\"Movie\",\"title\":\"Oct-01\",\"director\":\"Kunle Afolayan\",\"cast\":\"Sadiq Daba, David Bailie, Kayode Olaiya, Kehinde Bankole, Fabian Adeoye Lojede, Nick Rhys, Kunle Afolayan, Colin David Reese, Ibrahim Shatta, Femi Adebayo, Kanayo O. Kanayo, Lawrence Stubbings, Ademola Adedoyin\",\"country\":\"Nigeria\",\"dateAdded\":\"2019-09-01T00:00:00\",\"releaseYear\":2014,\"rating\":\"TV-14\",\"duration\":\"149 min\",\"listedIn\":\"Dramas, International Movies, Thrillers\",\"description\":\"Against the backdrop of Nigeria's looming independence from Britain, detective Danladi Waziri races to capture a killer terrorizing local women.\"},{\"id\":\"s17\",\"type\":\"TV Show\",\"title\":\"Feb-09\",\"director\":\"\",\"cast\":\"Shahd El Yaseen, Shaila Sabt, Hala, Hanadi Al-Kandari, Salma Salem, Ibrahim Al-Harbi, Mahmoud Boushahri, Yousef Al Balushi, Ghorour, Abdullah Al-bloshi\",\"country\":\"\",\"dateAdded\":\"2019-03-20T00:00:00\",\"releaseYear\":2018,\"rating\":\"TV-14\",\"duration\":\"1 Season\",\"listedIn\":\"International TV Shows, TV Dramas\",\"description\":\"As a psychology professor faces Alzheimer's, his daughter and her three close female friends experience romance, marriage, heartbreak and tragedy.\"},{\"id\":\"s18\",\"type\":\"Movie\",\"title\":\"22-Jul\",\"director\":\"Paul Greengrass\",\"cast\":\"Anders Danielsen Lie, Jon Øigarden, Jonas Strand Gravli, Ola G. Furuseth, Maria Bock, Thorbjørn Harr, Jaden Smith\",\"country\":\"Norway, Iceland, United States\",\"dateAdded\":\"2018-10-10T00:00:00\",\"releaseYear\":2018,\"rating\":\"R\",\"duration\":\"144 min\",\"listedIn\":\"Dramas, Thrillers\",\"description\":\"After devastating terror attacks in Norway, a young survivor, grieving families and the country rally for justice and healing. Based on a true story.\"},{\"id\":\"s19\",\"type\":\"Movie\",\"title\":\"15-Aug\",\"director\":\"Swapnaneel Jayakar\",\"cast\":\"Rahul Pethe, Mrunmayee Deshpande, Adinath Kothare, Vaibhav Mangale, Jaywant Wadkar, Satish Pulekar, Naina Apte, Uday Tikekar\",\"country\":\"India\",\"dateAdded\":\"2019-03-29T00:00:00\",\"releaseYear\":2019,\"rating\":\"TV-14\",\"duration\":\"124 min\",\"listedIn\":\"Comedies, Dramas, Independent Movies\",\"description\":\"On India's Independence Day, a zany mishap in a Mumbai chawl disrupts a young love story while compelling the residents to unite in aid of a little boy.\"},{\"id\":\"s20\",\"type\":\"Movie\",\"title\":\"'89\",\"director\":\"\",\"cast\":\"Lee Dixon, Ian Wright, Paul Merson\",\"country\":\"United Kingdom\",\"dateAdded\":\"2018-05-16T00:00:00\",\"releaseYear\":2017,\"rating\":\"TV-PG\",\"duration\":\"87 min\",\"listedIn\":\"Sports Movies\",\"description\":\"Mixing old footage with interviews, this is the story of Arsenal's improbable win versus Liverpool in the final moments of the 1989 championship game.\"}],\"links\":{\"next\":\"https://netflix-app.openwrks.com/v1/Shows?page=2&limit=20\"},\"pagination\":{\"page\":1,\"limit\":20,\"total\":7787}}"
            };
            _context.Reviews.AddRange(examples);
            var pagination = new PaginationFilter
            {
                Limit = 20,
                PageNumber = 1
            };
            //Act
            OpenWrksApiService.Setup(x => x.GetShows(pagination)).ReturnsAsync(response);
            var act = _sut.GetShowsWithReviews(pagination);
            //Assert
            Assert.IsNotEmpty(act.Result);
            Assert.IsInstanceOf<List<ShowWithReview>>(act.Result);
        }
    }
}