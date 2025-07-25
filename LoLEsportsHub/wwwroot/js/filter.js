function filterTournaments() {
    const searchInput = document.getElementById("searchBar").value.toLowerCase();
    const regionFilter = document.getElementById("cityFilter").value.toLowerCase();
    const tournaments = document.querySelectorAll(".tournament-card");

    tournaments.forEach(card => {
        const tournamentName = card.querySelector(".card-title").textContent.toLowerCase();
        const tournamentRegion = card.dataset.region;

        const matchesSearch = tournamentName.includes(searchInput);
        const matchesRegion = regionFilter === "" || tournamentRegion === regionFilter;

        card.style.display = (matchesSearch && matchesRegion) ? "block" : "none";
    });
}