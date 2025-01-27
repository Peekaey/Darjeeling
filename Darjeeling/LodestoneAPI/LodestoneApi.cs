using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;
using Darjeeling.Interfaces;
using Darjeeling.Models;
using Darjeeling.Models.LodestoneEntities;

namespace Darjeeling.Helpers.LodestoneHelpers;

// Currently just uses mostly css selector parsing for elements
// TODO - Refactor to use AngleSharp Parsing

// TODO - Create FC Name Getter while using FCID Only to Inject into GetFreeCompanyMemberList
public class LodestoneApi : ILodestoneApi
{
    
    private string GenerateCharacterSearchQueryURL(string firstName, string lastName, string world)
    {
        // Example Query In Browser
        // https://na.finalfantasyxiv.com/lodestone/character/?q=Art+Bayard&worldname=Carbuncle&classjob=&race_tribe=&blog_lang=ja&blog_lang=en&blog_lang=de&blog_lang=fr&order=
        var baseCharacterSearchURL = "https://na.finalfantasyxiv.com/lodestone/character/?q=";
        var endQueryURL = $"&worldname={world}&classjob=&race_tribe=&blog_lang=ja&blog_lang=en&blog_lang=de&blog_lang=fr&order=";
        return $"{baseCharacterSearchURL}{firstName}+{lastName}{endQueryURL}";
    }

    private string GenerateFreeCompanySearchQueryURL(string freeCompanyId)
    {
        // Example Query In Browser
        // https://na.finalfantasyxiv.com/lodestone/freecompany/9229705223830889096/member/?page=1
        return $"https://na.finalfantasyxiv.com/lodestone/freecompany/{freeCompanyId}/member/?page=";
    }

    public async Task<LodestoneWebResult> GetLodestoneCharacterFreeCompany(string firstName, string lastName, string world)
    {
        
        try
        {
            var queryUrl = GenerateCharacterSearchQueryURL(firstName, lastName, world);
            var content = await GetLodestoneWebPageContent(queryUrl);

            if (string.IsNullOrEmpty(content))
            {
                return LodestoneWebResult.AsFailure("Unable to get web content");
            }

            var context = BrowsingContext.New(Configuration.Default);
            var document = await context.OpenAsync(req => req.Content(content));

            var linkElement = document.QuerySelector("a.entry__link");
            if (linkElement == null)
            {
                return LodestoneWebResult.AsFailure("No Character found with this name on the specified world");
            }

            var href = linkElement.GetAttribute("href");
            if (string.IsNullOrEmpty(href))
            {
                return LodestoneWebResult.AsFailure("Error Getting Character URL");
            }

            var match = Regex.Match(href, @"/lodestone/character/(\d+)/");
            if (!match.Success)
            {
                return LodestoneWebResult.AsFailure("Error Parsing Character ID");
            }
            
            var freeCompanyElement = document.QuerySelector("a.entry__freecompany__link span");
            if (freeCompanyElement == null)
            {
                return LodestoneWebResult.AsFailure("Character not in a free company");
            }

            string freeCompany = freeCompanyElement.TextContent;
            return LodestoneWebResult.AsSuccess(freeCompany);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error getting Free Company: " + ex);
            return LodestoneWebResult.AsFailure($"Exception occured when getting Free Company");
        }
    }

    public async Task<LodestoneWebResult> GetLodestoneCharacterId(string firstName, string lastName, string world)
    {
        
        try
        {
            var queryUrl = GenerateCharacterSearchQueryURL(firstName, lastName, world);
            var content = await GetLodestoneWebPageContent(queryUrl);

            if (string.IsNullOrEmpty(content))
            {
                LodestoneWebResult.AsFailure("Unable to get web content");
            }

            var context = BrowsingContext.New(Configuration.Default);
            var document = await context.OpenAsync(req => req.Content(content));

            var linkElement = document.QuerySelector("a.entry__link");
            if (linkElement == null)
            {
                return LodestoneWebResult.AsFailure("No Character found with this name on the specified world");
            }

            var href = linkElement.GetAttribute("href");
            var match = Regex.Match(href, @"/lodestone/character/(\d+)/");

            if (!match.Success)
            {
                return LodestoneWebResult.AsFailure("Error Parsing Character ID");
            }
            return LodestoneWebResult.AsSuccess(match.Groups[1].Value);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Exception getting Character ID: " + ex);
            return LodestoneWebResult.AsFailure($"Error: {ex.Message}");
        }
    }

    public async Task<LodestoneFCWebResult> GetLodestoneFreeCompanyMembers(string fcid)
    {
        var memberList = new List<LodestoneFCMember>();
        string initialUrl = GenerateFreeCompanySearchQueryURL(fcid);

        string content = await GetLodestoneWebPageContent(initialUrl);
        if (string.IsNullOrEmpty(content))
        {
            return LodestoneFCWebResult.AsFailure("Error getting initial Free Company page");
        }

        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(content));

        int totalPageCount = CalculateTotalMemberPageCount(document);

        if (totalPageCount == 0)
        {
            return LodestoneFCWebResult.AsFailure("Error calculating page count");
        }
        
        var freeCompanyName = GetLodestoneFreeCompanyName(document);


        for (int currentPage = 1; currentPage <= totalPageCount; currentPage++)
        {
            string urlQuery = $"{initialUrl}{currentPage}";
            memberList.AddRange(await GetMembersFromFreeCompanyProfile(urlQuery));
        }
        return LodestoneFCWebResult.AsSuccess(freeCompanyName, memberList);
    }

    private async Task<List<LodestoneFCMember>> GetMembersFromFreeCompanyProfile(string url)
    {
        var members = new List<LodestoneFCMember>();

        string content = await GetLodestoneWebPageContent(url);
        if (string.IsNullOrEmpty(content))
        {
            return members;
        }

        var context = BrowsingContext.New(Configuration.Default);
        var document = await context.OpenAsync(req => req.Content(content));
        
        foreach (var element in document.QuerySelectorAll("a.entry__bg"))
        {
            var href = element.GetAttribute("href");
            if (string.IsNullOrEmpty(href)) continue;

            var nameElement = element.QuerySelector(".entry__name");
            if (nameElement == null) continue;

            var fullName = nameElement.TextContent;
            var names = fullName.Split(' ');
            if (names.Length < 2) continue;

            var match = Regex.Match(href, @"/lodestone/character/(\d+)/");
            if (!match.Success) continue;

            members.Add(new LodestoneFCMember
            {
                FirstName = names[0],
                LastName = names[1],
                CharacterId = match.Groups[1].Value
            });
        }

        return members;
    }

    private int CalculateTotalMemberPageCount(IDocument document)
    {
        var pagerElement = document.QuerySelector("li.btn__pager__current");
        if (pagerElement == null) return 0;

        var match = Regex.Match(pagerElement.TextContent, @"Page \d+ of (\d+)");
        return match.Success ? int.Parse(match.Groups[1].Value) : 0;
    }

    private string GetLodestoneFreeCompanyName(IDocument document)
    {
        var freeCompanyNameElement = document.QuerySelector(".entry__freecompany__name");

        if (freeCompanyNameElement == null)
        {
            return "";
        }

        var match = freeCompanyNameElement.TextContent;
        return match;
    }

    private async Task<string> GetLodestoneWebPageContent(string url)
    {
        try
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching web content: " + ex);
            return "";
        }
    }



}