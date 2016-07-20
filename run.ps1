Param(
    [Parameter(Mandatory=$True,ParameterSetName="Compose")]
    [switch]$Compose,
    [Parameter(Mandatory=$True,ParameterSetName="Build")]
    [switch]$Build,
    [Parameter(Mandatory=$True,ParameterSetName="Clean")]
    [switch]$Clean
)

function Clean () {
    docker-compose down --rmi all

    $danglingImages = $(docker images -q --filter 'dangling=true')
    if (-not [String]::IsNullOrWhiteSpace($danglingImages)) {
        docker rmi -f $danglingImages
    }
}

function PublishAllProjects () {
    Get-ChildItem . `
        | where { $_.psiscontainer } `
        | where { (Test-Path (Join-Path $_.fullname "project.json")) } `
        | foreach {
            cd $_.fullname
            Remove-Item -Recurse -Force -ErrorAction SilentlyContinue artifacts
            dotnet publish -o artifacts
            cd ..
        }
}

function BuildImages () {
    docker-compose build
}

function Compose () {
    docker-compose kill
    docker-compose up
}

if($Compose) 
{
    Compose
} 
elseif($Build)
{
    PublishAllProjects
    BuildImages
}
elseif ($Clean)
{
    Clean
}