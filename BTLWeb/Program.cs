using BTLWeb.Models;
using BTLWeb.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using static Org.BouncyCastle.Math.EC.ECCurve;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("QlbanMayAnhContext");
builder.Services.AddDbContext<QlbanMayAnhContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddScoped<IELoaiSpRepository, LoaiSpRepository>();
builder.Services.AddScoped<IHangSxRepository, HangSxRepository>();
builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
});

builder.Services.AddDistributedMemoryCache();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseCors(options =>
           options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}");

app.Run();
