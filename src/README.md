## 💾 Storing the Secret (User Secrets)
From the project directory:

```bash
dotnet user-secrets set "JwtSettings:Secret" "<PASTE_GENERATED_SECRET_HERE>"
```
```bash
dotnet user-secrets set "JwtSettings:Secret" "7F3A9C6E2A4D9B8F..."
```

## ✅ Verifying the Secret

To confirm the secret is stored:

```bash
dotnet user-secrets list
```