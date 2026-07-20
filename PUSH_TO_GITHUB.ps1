
$ErrorActionPreference = "Stop"

$Repo = "https://github.com/usstunlab1/BlueLuck-event.git"

if (-not (Test-Path ".git")) {
    git init
    git branch -M main
}

$origin = git remote get-url origin 2>$null
if ($LASTEXITCODE -ne 0) {
    git remote add origin $Repo
}
elseif ($origin -ne $Repo) {
    git remote set-url origin $Repo
}

git add .
git commit -m "feat: generate BlueLuck minimal event foundation"
git push -u origin main
