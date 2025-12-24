using System;
using System.Threading.Tasks;

namespace FIsionAPI.Business.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<bool> Commit();
}