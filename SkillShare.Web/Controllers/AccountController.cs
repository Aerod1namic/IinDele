using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillShare.Web.Data;
using SkillShare.Web.Models;
using SkillShare.Web.ViewModels;

namespace SkillShare.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Age = model.Age,
                    Gender = model.Gender,
                    City = model.City,
                    AboutMe = model.AboutMe
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> CreateTestUsers()
        {
            var testUsers = new List<(string Email, string FirstName, string LastName, int Age, string Gender, string City, string AboutMe)>
            {
                ("john.doe@test.com", "John", "Doe", 28, "Male", "New York", "Experienced software developer with passion for teaching"),
                ("anna.smith@test.com", "Anna", "Smith", 24, "Female", "Los Angeles", "Professional designer looking to learn programming"),
                ("mike.brown@test.com", "Mike", "Brown", 32, "Male", "Chicago", "Math teacher interested in computer science"),
                ("sarah.wilson@test.com", "Sarah", "Wilson", 27, "Female", "Boston", "Digital marketing specialist wanting to learn web development"),
                ("alex.johnson@test.com", "Alex", "Johnson", 30, "Male", "San Francisco", "Full-stack developer offering mentorship in web technologies")
            };

            foreach (var testUser in testUsers)
            {
                var user = new ApplicationUser
                {
                    UserName = testUser.Email,
                    Email = testUser.Email,
                    FirstName = testUser.FirstName,
                    LastName = testUser.LastName,
                    Age = testUser.Age,
                    Gender = testUser.Gender,
                    City = testUser.City,
                    AboutMe = testUser.AboutMe,
                    EmailConfirmed = true
                };

                var existingUser = await _userManager.FindByEmailAsync(testUser.Email);
                if (existingUser == null)
                {
                    var result = await _userManager.CreateAsync(user, "Test123!");
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }

        private async Task CreateTestSkills()
        {
            var skills = new List<(string Name, string Category, string Description)>
            {
                ("C# Programming", "Programming", "Object-oriented programming with C#"),
                ("ASP.NET Core", "Web Development", "Web development using ASP.NET Core framework"),
                ("JavaScript", "Programming", "Frontend development with JavaScript"),
                ("UI/UX Design", "Design", "User interface and experience design"),
                ("Python", "Programming", "Python programming language basics"),
                ("Digital Marketing", "Marketing", "Digital marketing strategies and tools"),
                ("SQL", "Database", "Database management with SQL"),
                ("Mathematics", "Education", "Advanced mathematics"),
                ("React", "Web Development", "Frontend development with React"),
                ("Node.js", "Programming", "Backend development with Node.js")
            };

            foreach (var skillInfo in skills)
            {
                var existingSkill = await _context.Skills
                    .FirstOrDefaultAsync(s => s.Name == skillInfo.Name);

                if (existingSkill == null)
                {
                    _context.Skills.Add(new Skill
                    {
                        Name = skillInfo.Name,
                        Category = skillInfo.Category,
                        Description = skillInfo.Description
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<IActionResult> CreateTestUsersWithSkills()
        {
            await CreateTestSkills();

            var testUsers = new List<(string Email, string FirstName, string LastName, int Age, string Gender, string City, string AboutMe, string[] TeachingSkills, string[] LearningSkills)>
            {
                ("john.doe@test.com", "John", "Doe", 28, "Male", "New York", 
                "Experienced software developer with passion for teaching",
                new[] { "C# Programming", "ASP.NET Core", "JavaScript" },
                new[] { "UI/UX Design", "Digital Marketing" }),

                ("anna.smith@test.com", "Anna", "Smith", 24, "Female", "Los Angeles",
                "Professional designer looking to learn programming",
                new[] { "UI/UX Design" },
                new[] { "C# Programming", "JavaScript", "Python" }),

                ("mike.brown@test.com", "Mike", "Brown", 32, "Male", "Chicago",
                "Math teacher interested in computer science",
                new[] { "Mathematics" },
                new[] { "Python", "JavaScript" }),

                ("sarah.wilson@test.com", "Sarah", "Wilson", 27, "Female", "Boston",
                "Digital marketing specialist wanting to learn web development",
                new[] { "Digital Marketing" },
                new[] { "JavaScript", "React", "HTML/CSS" }),

                ("alex.johnson@test.com", "Alex", "Johnson", 30, "Male", "San Francisco",
                "Full-stack developer offering mentorship in web technologies",
                new[] { "JavaScript", "React", "Node.js", "SQL" },
                new[] { "Python", "UI/UX Design" })
            };

            foreach (var testUser in testUsers)
            {
                var existingUser = await _userManager.FindByEmailAsync(testUser.Email);
                if (existingUser == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = testUser.Email,
                        Email = testUser.Email,
                        FirstName = testUser.FirstName,
                        LastName = testUser.LastName,
                        Age = testUser.Age,
                        Gender = testUser.Gender,
                        City = testUser.City,
                        AboutMe = testUser.AboutMe,
                        EmailConfirmed = true
                    };

                    var result = await _userManager.CreateAsync(user, "Test123!");
                    if (result.Succeeded)
                    {
                        // Add teaching skills
                        foreach (var skillName in testUser.TeachingSkills)
                        {
                            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Name == skillName);
                            if (skill != null)
                            {
                                user.TeachingSkills.Add(skill);
                            }
                        }

                        // Add learning skills
                        foreach (var skillName in testUser.LearningSkills)
                        {
                            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Name == skillName);
                            if (skill != null)
                            {
                                user.LearningSkills.Add(skill);
                            }
                        }

                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
} 