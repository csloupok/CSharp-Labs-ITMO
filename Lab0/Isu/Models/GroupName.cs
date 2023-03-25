using System.Text.RegularExpressions;
using Isu.Tools;

namespace Isu.Models;

public class GroupName
{
    private string _name;

    public GroupName(string name)
    {
        if (!ValidateGroupName(name))
            throw new IsuException("Incorrect group name!");
        _name = name;
    }

    public string Name => _name;

    private static bool ValidateGroupName(string groupName)
    {
        if (string.IsNullOrWhiteSpace(groupName))
            throw new IsuException("Name is empty or null!");
        return Regex.IsMatch(groupName, "^[A-Z][0-9]{1}[1-4]{1}[0-9]{3}$");
    }
}