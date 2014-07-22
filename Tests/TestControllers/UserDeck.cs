using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Pagan.Commands;
using Pagan.Queries;
using Pagan.Registry;
using Pagan.Tests.Fakes;

namespace Pagan.Tests.TestControllers
{
    public class UserDeck
    {
        public Table UserDecks { get; set; }
        
        [DbGenerated]
        public Column Id { get; set; }
        public Column Email { get; set; }
        public Column Name { get; set; }
        public Column LastUpdated { get; set; }
        public Column Deck { get; set; }
        public Column StudyId { get; set; }

        public Command Insert(string email, string name, string deck)
        {
            return UserDecks.Insert(Email.Is(email), Name.Is(name), Deck.Is(deck));
        }

        public Command Update(int id, string name, string deck)
        {
            return UserDecks.Update(Name.Is(name), Deck.Is(deck)).Where(Id == id);
        }

        public Command Update(dynamic deck)
        {
            return Update(deck.Id, deck.Name, deck.Deck);
        }

        public Command Delete(int id)
        {
            return UserDecks.Delete().Where(Id == id);
        }

        public Command Delete(dynamic deck)
        {
            return Delete((int)deck.Id);
        }

        public Query GetDeck(int id)
        {
            return UserDecks.Where(Id == id);
        }
    }

    [TestFixture]
    public class CommandTests
    {
        private Table<UserDeck> _table;

        [SetUp]
        public void Setup()
        {
            _table = new Table<UserDeck>(new Mock<ITableFactory>().Object, new FakeConventions());
        }

        [Test]
        public void DoesAutoIdWork()
        {
            using (var db = new Database("TouchpointsWeb"))
            {
                var id =
                    db.Command<UserDeck>(x => x.Insert("demo@zohub.net", "pete", "{ \"data\": \"some deck data\" }"));

                var deck = db.Query<UserDeck>(x => x.GetDeck(id)).First();

                deck.Name += " Warner";

                db.Command<UserDeck>(x => x.Update(deck));

                db.Command<UserDeck>(x => x.Delete(deck));

            }
            
        }


        [Test]
        public void DeleteAll()
        {
            using (var db = new Database("TouchpointsWeb"))
            {
                db.Command<UserDeck>(x => x.UserDecks.Delete());
            }
        }

        [Test]
        public void UpdateMany()
        {

            using (var db = new Database("TouchpointsWeb"))
            {
                db.Command<UserDeck>(x => x.Insert("demo@zohub.net", "pete", "item1"));
                db.Command<UserDeck>(x => x.Insert("demo@zohub.net", "pete", "item2"));
                db.Command<UserDeck>(x => x.Insert("demo@zohub.net", "pete", "item3"));

                db.Command<UserDeck>(x => x.UserDecks.Update(x.Name.Is("Pete")).Where(x.Email == "demo@zohub.net"));

                var peteDecks = db.Query<UserDeck>(x => x.UserDecks.Where(x.Email == "demo@zohub.net"));
            }
        }
    }
}
