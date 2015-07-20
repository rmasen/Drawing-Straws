' The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

''' <summary>
''' A basic page that provides characteristics common to most applications.
''' </summary>
Public NotInheritable Class BasicPage1
    Inherits Common.LayoutAwarePage

        ''' <summary>
        ''' Populates the page with content passed during navigation.  Any saved state is also
        ''' provided when recreating a page from a prior session.
        ''' </summary>
        ''' <param name="navigationParameter">The parameter value passed to
        ''' <see cref="Frame.Navigate"/> when this page was initially requested.
        ''' </param>
        ''' <param name="pageState">A dictionary of state preserved by this page during an earlier
        ''' session.  This will be null the first time a page is visited.</param>
        Protected Overrides Sub LoadState(navigationParameter As Object, pageState As Dictionary(Of String, Object))
        If pageState IsNot Nothing AndAlso pageState.ContainsKey("playerName") Then
            txtPlayerName.Text = pageState.Values("playerName").ToString
        End If
        If pageState IsNot Nothing AndAlso pageState.ContainsKey("teamSize") Then
            txtNumOfPlayers.Text = pageState.Values("teamSize").ToString
        End If
        
    End Sub


        ''' <summary>
        ''' Preserves state associated with this page in case the application is suspended or the
        ''' page is discarded from the navigation cache.  Values must conform to the serialization
        ''' requirements of <see cref="Common.SuspensionManager.SessionState"/>.
        ''' </summary>
        ''' <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        Protected Overrides Sub SaveState(pageState As Dictionary(Of String, Object))
        pageState.Add("lstAddedPlayers", lstAddedPlayers)
        pageState.Add("lstTeams", lstPairedPlayers)
    End Sub



    Dim players As New List(Of Player)
    Dim teams As New List(Of List(Of Player))
    Dim teamNames As New List(Of String) From {"Team Awesome", "Team Robots", "Team Pizza", "Team Tacos", "Team America", "The A Team", "The Warriors", _
                                               "The Norsemen", "The Ladykillers", "Team Zissou", "The Grifters", "The Watchmen", "The Fellowship", _
                                               "Deadly Viper Assassination Squad", "Space Marines", "The X-Men", "The Avengers", "TMNT", "The Incredibles", _
                                               "The Rebel Alliance", "The Ghostbusters", "The Goonies", "The Globetrotters", "The Bad News Bears", _
                                               "Hackensack Bulls", "The Baseball Furies", "ToonSquad", "Average Joe's", "The Riffs"}


    Private Sub AddedPlayersList()
        lstAddedPlayers.Items.Clear()
        For Each p As Player In players
            lstAddedPlayers.Items.Add(p.ToString)
        Next
        txtPlayerName.Text = ""
        txtPlayerName.Focus(Windows.UI.Xaml.FocusState.Keyboard)
    End Sub

    Private Sub pairedPlayerList()
        lstPairedPlayers.Items.Clear()
        Dim rand As New Random
        For Each item As List(Of Player) In teams
            Dim team As Integer = rand.Next(0, teamNames.Count)
            lstPairedPlayers.Items.Add(teamNames(team))
            For Each pl As Player In item
                lstPairedPlayers.Items.Add("    " & pl.ToString)
            Next
            lstPairedPlayers.Items.Add(" ")
        Next
        teams.Clear()
    End Sub

    Private Sub btnSort_Click(sender As Object, e As RoutedEventArgs) Handles btnSort.Click
        Dim orderedPlayers As IEnumerable(Of Player) = _
            players.OrderBy(Function(player) player.index)
        Dim teamSize As Integer = CInt(txtNumOfPlayers.Text)
        Dim numOfTeams As Integer
        If orderedPlayers.Count Mod teamSize = 0 Then
            lblError.Text = ""
            numOfTeams = (orderedPlayers.Count / teamSize)
        Else
            lblError.Text = "The number of players you have added do not equally divide into teams"
        End If
        Dim counter As Integer = 0
        For itter As Integer = 1 To numOfTeams
            Dim team As New List(Of Player)
            For ct As Integer = 1 To teamSize

                team.Add(orderedPlayers(counter))
                counter += 1
            Next
            teams.Add(team)
        Next
        pairedPlayerList()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As RoutedEventArgs) Handles btnAdd.Click
        If txtPlayerName.Text <> "" Then
            players.Add(New Player(txtPlayerName.Text))
        End If

        AddedPlayersList()
    End Sub

    Private Sub PlayerInput_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtPlayerName.TextChanged
        Dim roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings
        roamingSettings.Values("playerName") = txtPlayerName.Text
    End Sub

    Private Sub TeamSize_TextChanged(sender As Object, e As TextChangedEventArgs) Handles txtNumOfPlayers.TextChanged
        Dim roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings
        roamingSettings.Values("teamSize") = txtPlayerName.Text
    End Sub

    Private Sub btnClear_Click(sender As Object, e As RoutedEventArgs) Handles btnClearPlayers.Click
        players.Clear()
        AddedPlayersList()
    End Sub

    Private Sub btnClearTeams_Click(sender As Object, e As RoutedEventArgs) Handles btnClearTeams.Click
        teams.Clear()
        For Each pl As Player In players
            pl.changeIndex(pl)
        Next
        pairedPlayerList()
    End Sub
End Class
