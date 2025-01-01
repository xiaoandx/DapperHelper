using System.Collections.Generic;
using System.Text;

namespace DapperHelpers.Core.Models
{
  /// <summary>
  /// 命令集合
  /// </summary>
  /// <remarks>
  /// Author：Willis
  /// <para>Version: v1.6.0.1</para>
  /// </remarks>
  public class UserCommands
  {
    /// <summary>
    /// 命令集合
    /// </summary>
    private readonly List<UserCommand> userCommands = new List<UserCommand>();

    /// <summary>
    /// userCommands 命令集合的个数
    /// </summary>
    public int Count
    {
      get { return userCommands.Count; }
    }

    /// <summary>
    /// 添加一个SQL
    /// </summary>
    /// <param name="commandText"></param>
    /// <param name="keepSemicolon">是否在SQL结束地方保留分号 ； </param>
    public void Add(string commandText, bool keepSemicolon = false)
    {
      if (keepSemicolon == false)
      {
        commandText = commandText.Trim().Trim(';');
      }

      userCommands.Add(new UserCommand(commandText));
    }

    /// <summary>
    /// 添加一个参数化SQL
    /// </summary>
    /// <param name="userCommand"></param>
    /// <param name="keepSemicolon">是否在SQL结束地方保留分号 ； </param>
    public void Add(UserCommand userCommand, bool keepSemicolon = false)
    {
      if (!keepSemicolon)
      {
        userCommand.CommandText = userCommand.CommandText.Trim().Trim(';');
      }

      userCommands.Add(userCommand);
    }

    /// <summary>
    /// 批量添加参数化SQL
    /// </summary>
    /// <param name="commands"></param>
    public void AddRange(UserCommands commands)
    {
      this.userCommands.AddRange(commands.userCommands);
    }

    /// <summary>
    /// 获取命令集合
    /// </summary>
    /// <returns></returns>
    public IEnumerable<UserCommand> AsEnumerable()
    {
      foreach (UserCommand userCommand in userCommands)
      {
        yield return userCommand;
      }
    }

    /// <summary>
    /// 获取命令集合的字符串
    /// </summary>
    /// <returns></returns>
    public string CollectionToString()
    {
      StringBuilder sbSqLs = new StringBuilder();

      foreach (UserCommand userCommand in userCommands)
      {
        sbSqLs.Append(userCommand.CommandText + ";");
      }

      return sbSqLs.ToString();
    }

    /// <summary>
    /// 清空命令集合
    /// </summary>
    public void Clear()
    {
      userCommands.Clear();
    }
  }
}