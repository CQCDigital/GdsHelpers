param (

    [string] $sourceRepoUrl,             #required
    [string] $destRepoUrl,               #required

    [string] $sourceBranch,          #required
    [string] $destBranch             #required
)

$supplied_required_params = if(
            $sourceRepoUrl -And
            $destRepoUrl -And 
            $sourceBranch -And
            $destBranch -And
            $sourceRepoUrl -ne '' -And
            $destRepoUrl -ne '' -And
            $sourceBranch -ne '' -And
            $destBranch -ne ''
            ) {1} else {0} #consider using $var = (IF) ? 1 : 0


Write-Output "#### CodingInTheOpen.ps1: Supplied parameters: sourceRepoUrl: $sourceRepoUrl; destRepoUrl: $destRepoUrl;"

if($supplied_required_params) {

    filter timestamp {"$(Get-Date -Format G): $_"}

    Write-Output  "Start" | timestamp


    Write-Output  ""
    Write-Output  "----------------------------------------------"
    Write-Output ""


    Write-Output "Declare our variables" | timestamp

    $WorkingTempDir = "WorkingTempDir"
    $SourceTempDir = "SourceTempDir"
    $DestTempDir = "DestTempDir"


    Write-Output ""
    Write-Output "----------------------------------------------"
    Write-Output ""


    Write-Output "Create our Working Directory" | timestamp
    New-Item -Name $WorkingTempDir -ItemType directory | Out-Null

    Write-Output "Switch to Working Directory" | timestamp
    cd $WorkingTempDir

    Write-Output "Create Source Temp Directory" | timestamp
    New-Item -Name $SourceTempDir -ItemType directory | Out-Null

    Write-Output "Create Dest Temp Directory" | timestamp
    New-Item -Name $DestTempDir -ItemType directory | Out-Null


    Write-Output ""
    Write-Output "----------------------------------------------"
    Write-Output ""


    Write-Output "Switch to Source Directory" | timestamp
    cd $SourceTempDir

    $SourceDir = Get-Location
    Write-Output "Source - Save our Directory Path: $SourceDir" | timestamp


    Write-Output "Source - Cloning Master Branch" | timestamp
    git clone --quiet --single-branch --branch $sourceBranch $sourceRepoUrl .

    Write-Output "Source - Get Last Commit Message" | timestamp
    $LastSourceCommitMessage = git log -1 --pretty=%B

    Write-Output "Source - Remove .git folder" | timestamp
    Remove-Item -Recurse -Force .git


    Write-Output ""
    Write-Output "----------------------------------------------"
    Write-Output ""


    Write-Output "Switching to Working Directory" | timestamp
    cd ..


    Write-Output ""
    Write-Output "----------------------------------------------"
    Write-Output ""


    Write-Output "Switch to Dest Directory" | timestamp
    cd $DestTempDir

    $DestDir = Get-Location
    Write-Output "Dest - Save our Directory Path: $DestDir" | timestamp

    Write-Output "Dest - Cloning Master Branch" | timestamp
    git clone --quiet --single-branch --branch $destBranch $destRepoUrl .

    Write-Output "Dest - Remove everything excluding the .git folder" | timestamp
    Get-ChildItem -Exclude .git | Remove-Item -Recurse -Force


    Write-Output ""
    Write-Output "----------------------------------------------"
    Write-Output ""


    Write-Output "Switching to Working Directory" | timestamp
    cd ..


    Write-Output ""
    Write-Output "----------------------------------------------"
    Write-Output ""


    Write-Output "Copy from Source Temp Dir to Dest Temp Dir" | timestamp
    Copy-Item -Path "$SourceDir\*" -Destination  $DestDir -Recurse -Force -Exclude .git


    Write-Output ""
    Write-Output "----------------------------------------------"
    Write-Output ""


    Write-Output "Switch to Dest Directory" | timestamp
    cd $DestTempDir

    Write-Output "Setup Local User" | timestamp
    git config --global user.email "IsdAppDevSupport@cqc.org.uk"
    git config --global user.name "CQC Digital"

    Write-Output "Add Source Changes to Dest Repo" | timestamp
    git add .

    Write-Output "Commit changes to Dest Repo" | timestamp
    git commit -m "$LastSourceCommitMessage"

    Write-Output "Push Changes to Dest" | timestamp
    git push --quiet --set-upstream origin master


    Write-Output ""
    Write-Output "----------------------------------------------"
    Write-Output ""


    Write-Output "Switching to Working Directory" | timestamp
    cd ..

    Write-Output "Cleanup the files" | timestamp
    cd..
    Remove-Item $WorkingTempDir -Recurse -Force


    Write-Output ""
    Write-Output "----------------------------------------------"
    Write-Output ""


    Write-Output "Finished" | timestamp


}
else {

    Write-Output "##vso[task.logissue type=error]CodingInTheOpen.ps1 has missing parameters"
    throw "CodingInTheOpen.ps1: script terminated - missing parameter(s)."

}