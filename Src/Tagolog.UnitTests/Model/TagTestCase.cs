namespace Tagolog.UnitTests.Model
{
    public delegate void UseCaseAction( TagTestCase useCase, ITagScope scope );

    public class TagTestCase
    {
        public string Name { get; set; }
        public TagCollection TagsForCreateContext { get; set; }
        public TagCollection ExpectedTags { get; set; }

        /// <summary>
        /// Gets or set tagolog tag scope.
        /// </summary>
        public ITagScope Scope { get; set; }

        public UseCaseAction Action { get; set; }
    }
}
