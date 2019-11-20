using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Vtc_Freelancer.Models;

namespace Vtc_Freelancer.Services
{
  public class UserService
  {
    private MyDbContext dbContext;
    private HashPassword hashPassword;
    public UserService(MyDbContext dbContext, HashPassword hashPassword)
    {
      this.dbContext = dbContext;
      this.hashPassword = hashPassword;
    }
    public Users GetUsersByID(int? ID)
    {
      System.Console.WriteLine(ID);
      Users user = new Users();
      user = dbContext.Users.FirstOrDefault(u => u.UserId == ID);
      return user;
    }
    public bool Register(string username, string email, string password)
    {
      var user = dbContext.Users.FirstOrDefault(x => x.UserName == username);
      if (user != null)
      {
        return false;
      }
      else
      {
        try
        {
          Users User = new Users();
          User.UserName = username;
          User.Email = email;
          User.Password = password;
          User.RegisterDate = DateTime.Now;
          dbContext.Add(User);
          dbContext.SaveChanges();
          return true;
        }
        catch (System.Exception ex)
        {
          Console.WriteLine(ex.Message);
          return false;
        }
      }
    }
    // public Users GetUsers()
    public Users Login(string email, string password)
    {
      var character = "@";
      var user = new Users();
      if (email.Contains(character))
      {
        user = dbContext.Users.FirstOrDefault(u => u.Email == email);
        if (user != null)
        {
          if (user.Password == password)
          {
            // System.Console.WriteLine(user.UserName);
            return user;
          }

        }
        return null;
      }
      else
      {
        user = dbContext.Users.FirstOrDefault(u => u.UserName == email);
        if (user != null)
        {
          if (user.Password == password)
          {

            // dbContext.SaveChanges();
            return user;
          }

        }
        return null;
      }

    }
    public bool EditProfile(int Id, string Email, string UserName)
    {
      Users user = new Users();
      user = GetUserByUserId(Id);
      if (user != null)
      {
        user.UserName = UserName;
        user.Email = Email;
        dbContext.Update(user);
        dbContext.SaveChanges();
        return true;
      }

      return false;
    }
    public Users GetUserByUserId(int Id)
    {
      Users users = new Users();
      users = dbContext.Users.FirstOrDefault(u => u.UserId == Id);
      if (users != null)
      {
        return users;
      }
      else
      {
        return null;
      }

    }
    public Seller BecomeSeller(Users users, Languages languages, Seller seller1, Category category, Skills skills)
    {
      try
      {
        Seller seller = new Seller();
        seller.UserId = users.UserId;
        seller.SellerPoint = 0;
        seller.Description = seller1.Description;
        seller.RegisterDateSeller = DateTime.Now;
        dbContext.Add(seller);
        dbContext.SaveChanges();
        bool userlevel = UpdateUserLevel(users);
        Console.WriteLine(category.CategoryId);
        bool lang = AddLanguage(seller, languages);
        bool skill = AddSkills(seller, skills);
        Console.WriteLine(skills.SkillName);
        bool addcateseller = AddSellerCategory(seller, category);
        Console.WriteLine(category.CategoryId);
        if (lang)
        {
          if (addcateseller)
          {
            return seller;
          }

          Console.WriteLine("kkkkkkkk");

        }
        return null;


      }
      catch (System.Exception e)
      {
        Console.WriteLine(e.Message);
        return null;
        throw;
      }

    }
    public bool AddLanguage(Seller seller, Languages languages1)
    {
      try
      {

        Languages languages = new Languages();
        languages.SellerId = seller.SellerId;
        languages.Language = languages1.Language;
        languages.Level = languages1.Level;
        dbContext.Add(languages);
        dbContext.SaveChanges();
        return true;

      }
      catch (System.Exception e)
      {
        Console.WriteLine(e.Message);
        return false;
        throw;
      }


    }
    public bool AddSellerCategory(Seller seller, Category category)
    {
      try
      {
        SellerCategory sellerCategory = new SellerCategory();
        sellerCategory.SellerId = seller.SellerId;
        sellerCategory.CategoryId = category.CategoryId;
        Console.WriteLine(sellerCategory.SellerId);
        Console.WriteLine(sellerCategory.CategoryId);
        dbContext.Add(sellerCategory);
        dbContext.SaveChanges();
        return true;
      }
      catch (System.Exception e)
      {
        Console.WriteLine(e.Message);
        return false;
        throw;
      }
    }
    public bool UpdateUserLevel(Users users)
    {
      // Users users1 =  new Users();
      users.UserLevel = 1;
      dbContext.Update(users);
      dbContext.SaveChanges();
      return true;

    }
    public bool AddSkills(Seller seller, Skills skills1)
    {
      try
      {
        Skills skills = new Skills();
        skills.SkillName = skills1.SkillName;
        skills.SkillLevel = skills1.SkillLevel;
        skills.SellerId = seller.SellerId;
        dbContext.Add(skills);
        dbContext.SaveChanges();
        return true;
      }
      catch (System.Exception e)
      {
        Console.WriteLine(e.Message);
        return false;
        throw;
      }

    }




  }
}