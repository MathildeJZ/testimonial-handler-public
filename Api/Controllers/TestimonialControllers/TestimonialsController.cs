//LOCAL LIBRARIES
using Testimonial.Application.Features.Testimonials.Interfaces;
using Testimonial.Application.Features.SubjectEntries;
using Testimonial.Application.Features.Testimonials;
using Testimonial.Application.Features.Teachers;
using Testimonial.Application.Features.Subjects;
using Testimonial.Application.Features.Users;
using Testimonial.Infrastructure.Persistence;
using Testimonial.Application.Features;
//EXTERNAL LIBRARIES
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace Testimonial.Api.Controllers.TestimonialControllers;

[ApiController]
[Route("api/testimonials")]
public class TestimonialsController : ControllerBase
{
    private readonly ITestimonialService _testimonialService;
    private readonly ISubjectEntryService _subjectEntryService;
    private readonly ITestimonialPdfGenerator _pdfGenerator;
    private readonly AppDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public TestimonialsController(ITestimonialService testimonialService, ISubjectEntryService subjectEntryService, ITestimonialPdfGenerator pdfGenerator, AppDbContext context, ICurrentUserService currentUser)
    {
        _testimonialService = testimonialService;
        _subjectEntryService = subjectEntryService;
        _pdfGenerator = pdfGenerator;
        _context = context;
        _currentUser = currentUser;
    }

    // Opret vidnesbyrd
    //[Authorize(Roles = RoleNames.Admin + "," + RoleNames.ClassTeacher)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTestimonialRequest request)
    {
        var result = await _testimonialService.CreateTestimonialAsync(request);
        return Ok(new
        {
            result.TestimonialId
        });
    }

    //TEST PDF
    [HttpGet("test-pdf")]
    public IActionResult TestPdf()
    {
        var dto = new TestimonialDto
        {
            StudentFullName = "Asger Aaby Engelbredt",
            Year = 9,
            DocumentMonthYear = "Maj 2026",
            SchoolName = "Kristofferskolen",

            IntroParagraph = "I dette vidnesbyrd kan du læse, hvad der er gennemgået og arbejdet med i de enkelte fag i 9. klasse, og efterfølgende er der en beskrivelse af eleven i det givne fag. Først kommer her klasseledernes overordnede beskrivelse af eleven: ",
            OverallStudentDescription = "Asger er en engageret og nysgerrig elev, som deltager aktivt i undervisningen og bidrager positivt til klassens fællesskab. Han arbejder selvstændigt med sine opgaver og viser vilje til at fordybe sig i faglige problemstillinger. Asger møder velforberedt til undervisningen og tager ansvar for sin egen læring. Han er god til at samarbejde med sine klassekammerater og udviser respekt for andres perspektiver i fælles diskussioner. I løbet af skoleåret har Asger udviklet sine faglige kompetencer og arbejder målrettet med at forbedre både skriftlige og mundtlige fremstillinger. Han viser en stabil arbejdsindsats og har en positiv tilgang til nye udfordringer.",
            EndMessage = "Vi ønsker Asger alt det bedste på hans vej. ",

            Subjects = new List<SubjectSectionDto>
            {
                new SubjectSectionDto
                {
                    SubjectName = "Dansk og litteratur",
                    TeacherName = "Dia Flemming",
                    TeacherStatement = "Asger har god forståelse for danskfaget. Han har arbejdet godt med i dansktimerne og afleveret årets lektier. Asger er god til at formulere sig på skrift, han har god genreforståelse og staver godt. Kommateringen er det godt fortsat at træne. Asger kan lave et godt layout på skriftlige opgaver, han har god læsehastighed og læseforståelse. Han kan både arbejde alene og sammen med andre. Mundtligt har Asger ikke deltaget så meget i de ugentlige lektioner, men han har flere gange lavet nogle flotte mundtlige præstationer, bl.a. da han ved skolens Speakers’ Corner holdt en engageret og velargumenteret tale foran de fremmødte elever og lærere. Asger er gerne lidt tilbageholdende, men når han giver sig selv lov, vil han kunne stå meget stærkt foran et publikum. Asger har en rigtig god analytisk forståelse og kan reflektere selvstændigt over et emne. ",

                    DescriptionBlocks = new List<DescriptionBlockDto>
                    {
                        new DescriptionBlockDto
                        {
                            Heading = "Dansk og litteratur, hovedfag",
                            Body = "I første periode har vi arbejdet med den litterære periode ‘Romantikken’, samt ‘Romantismen’. Vi har talt om periodens ånd og særkender, både i samfundet og i litteraturen. På tekstsiden har eleverne gjort bekendtskab med bl.a. Fibiger og Oehlenschläger. Vi har arbejdet med at læse, lytte til og skrive digte. Eleverne trænede brug af ‘automatisk opdaterende indholdsfortegnelser’, overskrifter, sidetal, flot forside, kildehenvisninger m.m. Endelig arbejdede vi med intertekstualitet; til dette brugte vi en online undervisningsportal, og eleverne fik således også kendskab til dén form for undervisning. I anden hovedfagsperiode har vi arbejdet med Det moderne gennembrud og Georg Brandes. Det primære arbejde har centreret sig om tale- og argumentationsteori, samt elevernes egne taler. Efter gennemgang af teori af Toulmin og Cicero begav eleverne sig ud i at skrive en personlig tale. Vi hjalp hinanden med at sikre gode argumenter, brug af appelformer, samt at fastholde en rød tråd. Eleverne fik vejledning i gestik, mimik, generel fremtoning på en scene, artikulation, pauser m.m. Eleverne fremførte deres taler ved skolens eget Speakers’ Corner"
                        },
                        new DescriptionBlockDto
                        {
                            Heading = "Dansk fagtimer",
                            Body = "Eleverne har over året læst og analyseret 4 bøger. En af bøgerne blev analyseret og fremlagt, som var det en mundtlig eksamen. Vi har arbejdet med appelformer, sproglig stil og målgruppeforståelse. Vi har arbejdet med intertekstualitet i novellerne Irene Holm og Små frø. Eleverne har hørt om Herman Bang og impressionismen, og de har skrevet en impressionistisk reportage på baggrund af egne interviews af togpassagerer. Vi har arbejdet med layout på en avisartikel; øvet digte til recitation; hørt om billedanalyse; hørt nyhedspodcast m.fl. Vi har talt om identitet og om mandeidealer, og eleverne har lavet multimodale produktioner, kortfilm og tekst, over temaet ‘Et godt mandeideal’, og med teoretisk baggrund har de fokuseret særligt på enten lyd, billede, krop, ord eller perspektiv. Vi har jævnligt arbejdet med grammatik og kort med en svensk tekst, lyttet til svensk tale og øvet en svensk tungevrider. "
                        }
                    }
                },
                new SubjectSectionDto
                {
                    SubjectName = "Engelsk",
                    TeacherName = "Sussane Grange",
                    TeacherStatement = "Asger har et meget velfunderet niveau i engelsk. Skriftligt udtrykker han sig godt og frit i flere forskellige genrer. Asger har et veludviklet ordforråd og skriver varieret og tilpasser sit sprog til sin målgruppe. I gruppe- og pararbejde viser Asger altid seriøsitet og vilje til at få udført opgaven. Der er stadig behov for at bruge det talte sprog mere, men det skal siges, at Asger i løbet af året har gjort fremskridt og øvet sig i samtaler, ved oplæsning og ved fremlæggelser. Asger opfordres til at finde litteratur på engelsk i forskellige genrer og læse rigtig meget for at udvikle sit sprog. Asger er bestemt klar til nye udfordringer i engelsk.",

                    DescriptionBlocks = new List<DescriptionBlockDto>
                    {
                        new DescriptionBlockDto
                        {
                            Heading = "Fagbeskrivelse 1",
                            Body = "I engelsk har eleverne gennem året arbejdet med grammatik samt orddiktater med relevante ord. De har haft til opgave at være i gang med at læse en selvvalgt bog både hjemme og somme tider i klassen. To bøger har vi haft som klassesæt. De har arbejdet med forskellige emner og har i grupper eller individuelt udarbejdet en aflevering eller mundtlig præsentation. Der er blevet arbejdet med følgende emner. Under året har vi haft en samtale med en amerikaner over skype, som fortalte om det oprindelige folk i Amerika. Eleverne er blevet introduceret til Ted-talks, samt brugen af forskellige engelsksprogede hjemmesider med historisk og kulturel information. Der er blevet lagt vægt på at lytte til forskellige dialekter og talemåder, og på at udvide forståelsen for hvad der er britisk og amerikansk og af anden oprindelse.  "
                        }
                    }
                },
                new SubjectSectionDto
                {
                    SubjectName = "Samfundsfag",
                    TeacherName = "Marianne Søhager",
                    TeacherStatement = "Asger har et meget velfunderet niveau i engelsk. Skriftligt udtrykker han sig godt og frit i flere forskellige genrer. Asger har et veludviklet ordforråd og skriver varieret og tilpasser sit sprog til sin målgruppe. I gruppe- og pararbejde viser Asger altid seriøsitet og vilje til at få udført opgaven. Der er stadig behov for at bruge det talte sprog mere, men det skal siges, at Asger i løbet af året har gjort fremskridt og øvet sig i samtaler, ved oplæsning og ved fremlæggelser. Asger opfordres til at finde litteratur på engelsk i forskellige genrer og læse rigtig meget for at udvikle sit sprog. Asger er bestemt klar til nye udfordringer i engelsk.",

                    DescriptionBlocks = new List<DescriptionBlockDto>
                    {
                        new DescriptionBlockDto
                        {
                            Heading = "Fagbeskrivelse 1",
                            Body = "I engelsk har eleverne gennem året arbejdet med grammatik samt orddiktater med relevante ord. De har haft til opgave at være i gang med at læse en selvvalgt bog både hjemme og somme tider i klassen. To bøger har vi haft som klassesæt. De har arbejdet med forskellige emner og har i grupper eller individuelt udarbejdet en aflevering eller mundtlig præsentation. Der er blevet arbejdet med følgende emner. Under året har vi haft en samtale med en amerikaner over skype, som fortalte om det oprindelige folk i Amerika. Eleverne er blevet introduceret til Ted-talks, samt brugen af forskellige engelsksprogede hjemmesider med historisk og kulturel information. Der er blevet lagt vægt på at lytte til forskellige dialekter og talemåder, og på at udvide forståelsen for hvad der er britisk og amerikansk og af anden oprindelse.  "
                        }
                    }
                },
                new SubjectSectionDto
                {
                    SubjectName = "Idræt",
                    TeacherName = "Henrik Bagger",
                    TeacherStatement = "Asger har et meget velfunderet niveau i engelsk. Skriftligt udtrykker han sig godt og frit i flere forskellige genrer. Asger har et veludviklet ordforråd og skriver varieret og tilpasser sit sprog til sin målgruppe. I gruppe- og pararbejde viser Asger altid seriøsitet og vilje til at få udført opgaven. Der er stadig behov for at bruge det talte sprog mere, men det skal siges, at Asger i løbet af året har gjort fremskridt og øvet sig i samtaler, ved oplæsning og ved fremlæggelser. Asger opfordres til at finde litteratur på engelsk i forskellige genrer og læse rigtig meget for at udvikle sit sprog. Asger er bestemt klar til nye udfordringer i engelsk.",

                    DescriptionBlocks = new List<DescriptionBlockDto>
                    {
                        new DescriptionBlockDto
                        {
                            Heading = "Fagbeskrivelse 1",
                            Body = "I engelsk har eleverne gennem året arbejdet med grammatik samt orddiktater med relevante ord. De har haft til opgave at være i gang med at læse en selvvalgt bog både hjemme og somme tider i klassen. To bøger har vi haft som klassesæt. De har arbejdet med forskellige emner og har i grupper eller individuelt udarbejdet en aflevering eller mundtlig præsentation. Der er blevet arbejdet med følgende emner. Under året har vi haft en samtale med en amerikaner over skype, som fortalte om det oprindelige folk i Amerika. Eleverne er blevet introduceret til Ted-talks, samt brugen af forskellige engelsksprogede hjemmesider med historisk og kulturel information. Der er blevet lagt vægt på at lytte til forskellige dialekter og talemåder, og på at udvide forståelsen for hvad der er britisk og amerikansk og af anden oprindelse.  "
                        }
                    }
                }
            },
        };

        var pdf = _pdfGenerator.Generate(dto);
        return File(pdf, "application/pdf", "testimonial-example.pdf");
    }

    //Hent oprettet vidnesbyrd som PDF
    [HttpGet("{id}/pdf")]
    public async Task<IActionResult> GetPdf(int id)
    {
        var testimonial = await _context.Testimonials
            .Include(t => t.Student)
            .Include(t => t.SubjectEntries)
                .ThenInclude(se => se.Subject)
            .FirstOrDefaultAsync(t => t.TestimonialId == id);

        if (testimonial == null)
            return NotFound();

        var dto = new TestimonialDto
        {
            StudentFullName = testimonial.Student.FirstName + " " + testimonial.Student.LastName,
            Year = testimonial.SchoolYear,
            DocumentMonthYear = testimonial.DateCreated
                .ToString("MMMM yyyy", new CultureInfo("da-DK")),
        };

        var pdfBytes = _pdfGenerator.Generate(dto);

        return File(pdfBytes, "application/pdf", "vidnesbyrd.pdf");
    }


    // Hent vidnesbyrd
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _testimonialService.GetTestimonialByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("teachers")]
    public async Task<IActionResult> GetTeachers()
    {
        var teachers = await _context.Teacher
        .Select(t => new { t.TeacherId, t.FirstName, t.LastName })
        .ToListAsync();

        return Ok(teachers);
    }

    //FETCH TEACHER'S SUBJECTS\
    [HttpGet("me")]
    public async Task<ActionResult<SubjectTeacherDto>> GetMySubjects()
    {
        var userId = _currentUser.UserId;

        Console.WriteLine($"USER ID FROM TOKEN: '{userId}'");
        var allTeachers = await _context.Teacher.ToListAsync();

        var teacher = await _context.Teacher
            .FirstOrDefaultAsync(t => t.UserId == userId);

        Console.WriteLine($"USER ID: {userId}");
        Console.WriteLine($"TEACHER FOUND: {teacher != null}");

        if (teacher == null)
            return NotFound();

        var subjects = await _context.TeacherSubject
            .Where(ts => ts.TeacherId == teacher.TeacherId)
            .Select(ts => new SubjectDto
            {
                SubjectId = ts.SubjectId,
                Name = ts.Subject.Name
            })
            .ToListAsync();

        return Ok(new SubjectTeacherDto
        {
            TeacherId = teacher.TeacherId,
            Name = teacher.FirstName + teacher.LastName,
            Subjects = subjects
        });
    }

    // Hent fag efter klassetrin
    [HttpGet("subjects")]
    public async Task<IActionResult> GetSubjectsByGrade(int grade)
    {
        var subjects = await _context.Subjects
            .Where(s => s.Grade == grade)
            .Select(s => new
            {
                SubjectId = s.SubjectId,
                name = s.Name,
                Grade = s.Grade
            })
            .ToListAsync();

        return Ok(subjects);
    }


    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetByStudentId(int studentId)
    {
        var testimonials = await _context.Testimonials
            .Where(t => t.StudentId == studentId)
            .Select(t => new
            {
                testimonialId = t.TestimonialId,
                testimonial = t.OverallStudentDescription,
                schoolYear = t.SchoolYear,
                title = t.Title,
                grade = t.Grade,
                dateCreated = t.DateCreated
            })
            .ToListAsync();

        return Ok(testimonials);
    }
}