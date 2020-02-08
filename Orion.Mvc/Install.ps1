param($installPath, $toolsPath, $package, $project)


Foreach($item in $project.ProjectItems.Item("docs").ProjectItems)
{
	# set 'Copy To Output Directory' to 'Do not copy'
	$item.Properties.Item("CopyToOutputDirectory").Value = 0

	# set 'Build Action' to 'None'
	$item.Properties.Item("BuildAction").Value = 0
}

