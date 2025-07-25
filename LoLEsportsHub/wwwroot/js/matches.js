document.addEventListener("DOMContentLoaded", function () {
    console.log("✅ DOM fully loaded and parsed.");

    const modalElement = document.getElementById("matchDetailsModal");
    if (!modalElement) {
        console.error("❌ Error: Modal element #matchDetailsModal not found!");
        return;
    }

    const matchDetailsModal = new bootstrap.Modal(modalElement);
    const detailsContainer = document.getElementById("matchDetailsContent");

    // Attach event listeners to all 'View Details' buttons
    const viewDetailsButtons = document.querySelectorAll(".view-details-btn");
    if (viewDetailsButtons.length === 0) {
        console.warn("⚠️ No '.view-details-btn' elements found on the page.");
    }

    viewDetailsButtons.forEach(button => {
        button.addEventListener("click", function (event) {
            event.preventDefault();

            let matchId = this.getAttribute("data-match-id");
            console.log(`🎬 Fetching details for match ID: ${matchId}`);

            fetch(`/Match/DetailsPartial/${matchId}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error(`❌ Failed to load match details! Status: ${response.status}`);
                    }
                    return response.text();
                })
                .then(data => {
                    console.log("📥 Match details received:", data);

                    detailsContainer.innerHTML = data;

                    let matchTitleElement = detailsContainer.querySelector("h3");
                    let matchTitle = matchTitleElement ? matchTitleElement.textContent : "Match Details";
                    document.getElementById("matchDetailsLabel").textContent = matchTitle;

                    let modalBookmarksBtn = document.getElementById("add-to-bookmarks-btn");

                    if (modalBookmarksBtn) {
                        modalBookmarksBtn.setAttribute("data-match-id", matchId);

                        // Check if the current page is Watchlist, then hide the button
                        if (window.location.pathname.includes("/Bookmarks")) {
                            modalBookmarksBtn.style.display = "none";
                        } else {
                            // Make AJAX request to check if the match is already in the watchlist
                            fetch(`/Bookmarks/IsMatchInBookmarks/${matchId}`)
                                .then(response => response.json())
                                .then(isInBookmarks => {
                                    if (isInBookmarks) {
                                        modalBookmarksBtn.style.display = "none";
                                    } else {
                                        modalBookmarksBtn.style.display = "inline-block";
                                    }
                                })
                                .catch(error => {
                                    console.error("⚠️ Error checking watchlist status:", error);
                                });
                        }
                    }

                    detailsContainer.style.display = "block";
                    matchDetailsModal.show();

                    attachDynamicEventListeners();
                })
                .catch(error => {
                    console.error("❌ Error loading match details:", error);
                    detailsContainer.innerHTML = `<p class="text-danger">Failed to load match details. Please try again later.</p>`;
                });
        });
    });

    function attachDynamicEventListeners() {
        setTimeout(() => {
            console.log("🔄 Attaching dynamic event listeners...");

            $("#add-to-watchlist-btn").off("click").on("click", function () {
                let matchId = $(this).data("match-id");

                if (!matchId) {
                    Swal.fire("Error!", "Match ID is missing.", "error");
                    return;
                }

                $.post("/Bookmarks/Add", { matchId: matchId })
                    .done(function () {
                        Swal.fire({
                            title: "Added!",
                            text: "The match has been added to your watchlist.",
                            icon: "success",
                            confirmButtonColor: "#28a745"
                        });

                        $("#add-to-bookmarks-btn").hide(); // Hide button after adding
                    })
                    .fail(function () {
                        Swal.fire({
                            title: "Error!",
                            text: "Failed to add the match to your bookmarks. Please try again.",
                            icon: "error",
                            confirmButtonColor: "#dc3545"
                        });
                    });
            });

        }, 200);
    }
});

// jQuery for "Add to Watchlist" buttons in the match List
$(document).ready(function () {
    $(".add-to-watchlist-btn").on("click", function () {
        const matchId = $(this).data("match-id");

        if (!matchId) {
            Swal.fire("Error!", "Match ID is missing.", "error");
            return;
        }

        $.post("/Bookmarks/Add", { matchId: matchId })
            .done(function () {
                Swal.fire({
                    title: "Added!",
                    text: "The match has been added to your watchlist.",
                    icon: "success",
                    confirmButtonColor: "#28a745"
                });
            })
            .fail(function () {
                Swal.fire({
                    title: "Error!",
                    text: "Failed to add the match to your watchlist. Please try again.",
                    icon: "error",
                    confirmButtonColor: "#dc3545"
                });
            });
    });
});
