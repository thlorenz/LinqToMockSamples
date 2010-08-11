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

              // timStub.SetupGet()

                Tim = timStub.Object;
                Laura = lauraStub.Object;
            };

            it tims_name_is_tim = () => Tim.Name.ShouldEqual(TimsName);
        }
    }
}