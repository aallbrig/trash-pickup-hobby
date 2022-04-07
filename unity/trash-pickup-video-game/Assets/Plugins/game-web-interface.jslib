mergeInto(LibraryManager.library, {
  ConfigureAdvertisementSystem: function () {
    if (window.adConfig) {
      window.adConfig({
        sound: 'on',
      });
      window.console.log("Advertisement system configured");
    }
  },
  PlayStartGameAdvertisement: function () {
    if (window.adBreak) {
      window.adBreak({
        type: 'start',
        name: 'start-game',
        beforeAd: () => window.console.log("Start game before ad"),
        afterAd: () => window.console.log("Start game after ad")
      });
      window.console.log("Start game advertisement break played");
    }
  }
});