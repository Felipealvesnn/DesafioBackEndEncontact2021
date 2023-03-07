﻿
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TesteBackendEnContaact_Infra_IOC

{
    public static class INjecaoJWT
    {
        public static IServiceCollection AddSegurancaJWT(this IServiceCollection services,
            IConfiguration configuration)
        {
            //informar o tipo de autenticacao JWT-Bearer
            //definir o modelo de desafio de autenticacao
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //habilita a autenticacao JWT usando o esquema e desafio definidos
            //validar o token
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //valores validos
                    ValidIssuer = configuration["jwt:Issuer"],
                    ValidAudience = configuration["jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(configuration["jwt:SecretKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
            return services;
        }

    }
}