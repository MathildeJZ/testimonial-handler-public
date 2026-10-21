//external libraries
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
//local libraries
using Testimonial.Application.Features.Testimonials;
using Testimonial.Application.Features;

namespace Testimonial.Infrastructure.Features.Testimonials;

public class TestimonialDocument : IDocument
{
    private readonly TestimonialDto _d;

    public TestimonialDocument(TestimonialDto dto) => _d = dto;

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        // COVER PAGE
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(50);

            page.Content().Element(ComposeFrontPage);
        });

        // DOCUMENT PAGES
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(50);
            page.DefaultTextStyle(x => x.FontSize(11));

            page.Content().Column(col =>
            {
                col.Item().Element(ComposeIntroPage);

                foreach (var subject in _d.Subjects)
                {
                    col.Item().PageBreak();
                    col.Item().Element(c => ComposeSubjectSection(c, subject));
                }
            });

            page.Footer().Row(row =>
            {
                row.RelativeItem();

                row.RelativeItem()
                    .AlignCenter()
                    .Image("../frontend/public/KristofferskolenLogo.png");

                row.RelativeItem()
                    .AlignRight()
                    .Text(text =>
                    {
                        text.DefaultTextStyle(x =>
                            x.FontSize(10)
                            .FontColor(Colors.Grey.Darken1));

                        text.CurrentPageNumber();
                        text.Span(" af ");
                        text.TotalPages();
                    });
            });
        });
    }

    private void ComposeFrontPage(IContainer container)
    {
        container.Column(col =>
        {
            var titleStyle = TextStyle.Default
            .FontFamily("SchoolFrontPage")
            .FontSize(36)
            .FontColor(Colors.Grey.Darken3);

            col.Item()
                .AlignCenter()
                .PaddingTop(160)
                .Text("Vidnesbyrd")
                .Style(titleStyle)
                .FontSize(42);

            col.Item()
                .AlignCenter()
                .PaddingTop(10)
                .Text("for")
                .Style(titleStyle)
                .FontSize(24).Bold();

            col.Item()
                .AlignCenter()
                .PaddingTop(12)
                .Text(_d.StudentFullName)
                .Style(titleStyle)
                .FontSize(24).Bold();

            //year 9 layout
            col.Item().AlignCenter().PaddingTop(12)
                .Text($"{_d.Year}. KLASSE, {_d.DocumentMonthYear}")
                .Style(titleStyle)
                .FontSize(14);

            col.Item()
                .AlignCenter()
                .PaddingTop(80)
                .Text($"{_d.SchoolName}")
                .Style(titleStyle)
                .FontSize(14);

            col.Item()
                .AlignCenter()
                .PaddingTop(40)
                .Text("-")
                .Style(titleStyle)
                .FontSize(16);
        });
    }

    private void ComposeIntroPage(IContainer container)
    {
        container.Column(col =>
        {
            col.Spacing(10);

            col.Item().Text("Indledning").FontSize(16).Bold();
            col.Item().Text(_d.IntroParagraph);

            col.Item().PaddingTop(8).Text($"''{_d.OverallStudentDescription}''");
            col.Item().PaddingTop(20).Text(_d.EndMessage);
        });
    }

    private void ComposeSubjectSection(IContainer container, SubjectSectionDto s)
    {
        container.Column(col =>
        {
            col.Spacing(4);
            col.Item().Text(s.SubjectName).FontSize(18).Bold();
            col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten1);

            foreach (var block in s.DescriptionBlocks)
            {
                col.Item().PaddingTop(14).BorderBottom(1).BorderColor(Colors.Grey.Lighten1).PaddingBottom(10).Column(b =>
                {
                    b.Item().Text(block.Heading)
                    .FontSize(13)
                    .Bold();

                    b.Item().PaddingTop(4)
                    .Text(block.Body);
                });
            }

            col.Item().PaddingTop(16).Text($"Elevudtalelse i {s.SubjectName}").Bold();
            col.Item().PaddingTop(6).Text(s.TeacherStatement);

            col.Item().PaddingTop(20).AlignRight().Column(sig =>
            {
                sig.Item().Text(TeacherTitleHekper.GenerateTeacherTitle(s.SubjectName)).FontSize(12);
                sig.Item().Text(s.TeacherName).Italic().FontSize(15);
            });
        });
    }
}