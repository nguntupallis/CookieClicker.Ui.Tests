Feature: Cookie Clicker
  As a player I want to start a new game with a valid name So that I can play and earn cookies

  Scenario: Verify correct loading of the Cookie Clicker website
    Given I navigate to the Cookie Clicker website
    Then the page title should be "Cookie Clicker!"
    And there should be a `New Game` label
    And there should be a text input field labeled `Your name:`
    And there should be a `Start!` button
    And there should be a `High Scores` section

Scenario: Starting a new game
  Given I am on the Cookie clicker home page
  When I enter a <name>
  And I click the `Start` button
  Then the Cookie Clicker game page should load
  And the `Click Cookie!` button should be visible
  And the `Sell Cookies!` button should be visible
  And the `Buy Factories!` button should be visible
  
