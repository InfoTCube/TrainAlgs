using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class UserRepository : IUserRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public UserRepository(DataContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<MemberDto> GetMemberAsync(string username)
    {
        return await _context.Users
            .Where(user => user.UserName == username)
            .Include("Solutions.Author")
            .Include("UserRoles.Role")
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.SingleOrDefaultAsync(user => user.UserName == username);
    }

    public async Task<PagedList<SearchedMemberDto>> SearchForMemberAsync(ElementParams elementParams, string searchText)
    {
        var query = _context.Users
            .Where(user => user.NormalizedUserName.Contains(searchText.ToUpper()) == true)
            .Include("UserRoles.Role")
            .ProjectTo<SearchedMemberDto>(_mapper.ConfigurationProvider);

        return await PagedList<SearchedMemberDto>.CreateAsync(query, elementParams.PageNumber, elementParams.PageSize);
    }
    
    public void Update(AppUser user)
    {
        _context.Entry(user).State = EntityState.Modified;
    }
}