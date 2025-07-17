using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SmartLearn.Application.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace SmartLearn.Blazor.Services;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<string?> GetTokenAsync();
}

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new ApplicationException($"Login failed: {errorContent}");
        }

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        
        if (authResponse != null)
        {
            await _localStorage.SetItemAsync("authToken", authResponse.Token);
            await _localStorage.SetItemAsync("refreshToken", authResponse.RefreshToken);
            await _localStorage.SetItemAsync("user", authResponse.User);
            
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResponse.Token);
            
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyUserAuthentication(authResponse.User);
        }

        return authResponse!;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new ApplicationException($"Registration failed: {errorContent}");
        }

        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        
        if (authResponse != null)
        {
            await _localStorage.SetItemAsync("authToken", authResponse.Token);
            await _localStorage.SetItemAsync("refreshToken", authResponse.RefreshToken);
            await _localStorage.SetItemAsync("user", authResponse.User);
            
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authResponse.Token);
            
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyUserAuthentication(authResponse.User);
        }

        return authResponse!;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        await _localStorage.RemoveItemAsync("refreshToken");
        await _localStorage.RemoveItemAsync("user");
        
        _httpClient.DefaultRequestHeaders.Authorization = null;
        
        ((CustomAuthenticationStateProvider)_authenticationStateProvider).NotifyUserLogout();
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        return !string.IsNullOrEmpty(token);
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>("authToken");
    }
}