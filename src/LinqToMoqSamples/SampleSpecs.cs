using System.Linq;
using Machine.Specifications;
using Moq;
using it = Machine.Specifications.It;

namespace LinqToMoqSamples
{
    public class SampleSpecs
    {
        const string TimsName = "Tim";
        const int TimsAge = 30;
        const string LaurasName = "Laura";
        const int LaurasAge = 28;

        protected static IHusband Tim;
        protected static IWife Laura;

        public class when_tim_is_30_years_old_and_he_is_married_to_laura_who_is_28_years_old_and_stubs_are_prepared_via_setupget : SampleSpecs
        {
            Establish context = () => {
                var timStub = new Mock<IHusband>();
                timStub.SetupGet(t => t.Name).Returns(TimsName);
                timStub.SetupGet(t => t.Age).Returns(TimsAge);
                
                var lauraStub = new Mock<IWife>();
                lauraStub.SetupGet(l => l.Name).Returns(LaurasName);
                lauraStub.SetupGet(l => l.Age).Returns(LaurasAge);

                Tim = timStub.Object;
                Laura = lauraStub.Object;

                timStub.SetupGet(t => t.Wife).Returns(Laura);
                lauraStub.SetupGet(l => l.Husband).Returns(Tim);
            };

            Behaves_like<tim_and_laura> a_married_couple;
        }

        public class when_tim_is_30_years_old_and_he_is_married_to_laura_who_is_28_years_old_and_stubs_are_prepared_via_oneof : SampleSpecs
        {
            Establish context = () => {
                Tim = Mocks.OneOf<IHusband>(t => 
                    t.Name == TimsName &&
                    t.Age == TimsAge);

                Laura = Mocks.OneOf<IWife>(l => 
                    l.Name == LaurasName &&
                    l.Age == LaurasAge &&
                    l.Husband == Tim);

                Mock.Get(Tim).SetupGet(t => t.Wife).Returns(Laura);
            };

            Behaves_like<tim_and_laura> a_married_couple;
        }

        public class when_tim_is_30_years_old_and_he_is_married_to_laura_who_is_28_years_old_and_stubs_are_prepared_via_linq_method_chain : SampleSpecs
        {
            Establish context = () => {
                Tim = Mocks.Of<IHusband>().Where(t =>
                    t.Name == TimsName &&
                    t.Age == TimsAge)
                    .First();

               Laura = Mocks.Of<IWife>().Where(l =>
                     l.Name == LaurasName &&
                     l.Age == LaurasAge &&
                     l.Husband == Tim)
                     .First();
            
                Mock.Get(Tim).SetupGet(t => t.Wife).Returns(Laura);
            };

            Behaves_like<tim_and_laura> a_married_couple;
        }

        public class when_tim_is_30_years_old_and_he_is_married_to_laura_who_is_28_years_old_and_stubs_are_prepared_via_linq_inline : SampleSpecs
        {
            Establish context = () => {
                Tim = 
                    (from t in Mocks.Of<IHusband>()
                       where
                           t.Name == TimsName &&
                           t.Age == TimsAge
                       select t).First();
                Laura =
                    (from l in Mocks.Of<IWife>()
                        where
                            l.Name == LaurasName &&
                            l.Age == LaurasAge &&
                            l.Husband == Tim
                        select l).First();
                Mock.Get(Tim).SetupGet(t => t.Wife).Returns(Laura);
            };

            Behaves_like<tim_and_laura> a_married_couple;
        }

        public class when_tim_is_30_years_old_and_he_is_married_to_laura_who_is_28_years_old_and_stubs_are_prepared_via_linq_method_chain_in_parallel : SampleSpecs
        {
            Establish context = () =>
            {
                var marriedCoupleMocks =
                    Mocks.Of<IHusband>().SelectMany(
                        husband => Mocks.Of<IWife>(), (husband, wife) => new { husband, wife }).Where(
                            @t => @t.husband.Name == TimsName &&
                            @t.husband.Age == TimsAge &&
                            @t.wife.Name == LaurasName &&
                            @t.wife.Age == LaurasAge &&
                            @t.husband.Wife == @t.wife &&
                            @t.wife.Husband == @t.husband).Select(
                            @t => new { Husband = @t.husband, Wife = @t.wife });

                Tim = marriedCoupleMocks.First().Husband;
                Laura = marriedCoupleMocks.First().Wife;
            };

            Behaves_like<tim_and_laura> a_married_couple;
        }

        public class when_tim_is_30_years_old_and_he_is_married_to_laura_who_is_28_years_old_and_stubs_are_prepared_via_linq_inline_in_parallel : SampleSpecs
        {
            Establish context = () =>
            {
                var marriedCoupleMocks =
                    from husband in Mocks.Of<IHusband>()
                     from wife in Mocks.Of<IWife>()
                     where
                          husband.Name == TimsName &&
                          husband.Age == TimsAge && 
                          wife.Name == LaurasName &&
                          wife.Age == LaurasAge &&
                          husband.Wife == wife &&
                          wife.Husband == husband
                     select new { Husband = husband, Wife = wife };

                Tim = marriedCoupleMocks.First().Husband;
                Laura = marriedCoupleMocks.First().Wife;
            };

            Behaves_like<tim_and_laura> a_married_couple;
        }

        [Behaviors]
        public class tim_and_laura
        {
            it tims_name_is_tim = () => Tim.Name.ShouldEqual(TimsName);

            it tims_age_is_30 = () => Tim.Age.ShouldEqual(TimsAge);

            it tims_wifes_name_is_laura = () => Tim.Wife.Name.ShouldEqual(LaurasName);

            it tims_wifes_age_is_28 = () => Tim.Wife.Age.ShouldEqual(LaurasAge);

            it lauras_husbands_name_is_tim = () => Laura.Husband.Name.ShouldEqual(TimsName);

            it lauras_husbands_age_is_30 = () => Laura.Husband.Age.ShouldEqual(TimsAge);
        }
    }
}