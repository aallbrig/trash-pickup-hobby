const assert = require("assert");
const puppeteer = require("puppeteer");
const HEADLESS_MODE = process.env.HEADLESS_MODE || false;
const LANDING_PAGE = process.env.LANDING_PAGE || "http://localhost:8000";
const AMAZON_ASSOCIATES_TRACKING_ID = process.env.AMAZON_ASSOCIATES_TRACKING_ID || "allbright08-20";
const AMAZON_AD_SYSTEM_URL = process.env.AMAZON_AD_SYSTEM_URL || "//ws-na.amazon-adsystem.com";

describe("The landing page for the trash pickup hobby website", () => {
  let browser;

  before(async () => {
    browser = await puppeteer.launch({
      headless: HEADLESS_MODE,
      args: ['--disable-dev-shm-usage']
    });
  });

  after(async () => {
    await browser.close();
  });

  it("Should have a 🔥 page title", async () => {
    const page = await browser.newPage();
    await page.goto(LANDING_PAGE);
    let title = await page.title();
    assert.equal(title, "Trash Pickup Hobby");
  });

  it("Should have the correct amazon affiliates tracking ID in all amazon iframes 💰", async () => {
    const page = await browser.newPage();
    await page.goto(LANDING_PAGE);

    const frames = await page.frames();

    frames.find(f => {
      if (f.url().indexOf(AMAZON_AD_SYSTEM_URL) > -1) {
        const trackingIdQueryParam = new URLSearchParams(f.url()).get('tracking_id');
        assert.equal(
          trackingIdQueryParam === AMAZON_ASSOCIATES_TRACKING_ID,
          true,
          `The tracking ID ${trackingIdQueryParam} does not match the expected ${AMAZON_ASSOCIATES_TRACKING_ID}`
        );
      }
    });
  });

  it("Should link to the official trash pickup video game 🕹", async () => {
    const page = await browser.newPage();
    await page.goto(LANDING_PAGE);

    const videoGameLink = await page.$('#link-to-video-game');

    // If no HTML element is found, page.$ returns null
    assert.notEqual(videoGameLink, null);
  });

  it("Should contain a link to see the source code, for those curious 💻", async () => {
    const page = await browser.newPage();
    await page.goto(LANDING_PAGE);

    const linkToSourceCode = await page.$('#source-code-link');

    // If no HTML element is found, page.$ returns null
    assert.notEqual(linkToSourceCode, null);
  });

  it("Should contain a link to the author's website 🧑‍💻", async () => {
    const page = await browser.newPage();
    await page.goto(LANDING_PAGE);

    const linkToAuthorWebsite = await page.$('#author-website-link');

    // If no HTML element is found, page.$ returns null
    assert.notEqual(linkToAuthorWebsite, null);
  });

  it("Should display my current trash bag count", async () => {
    const page = await browser.newPage();
    await page.goto(LANDING_PAGE);

    const trashBagCount = await page.$('#trash-bag-count');

    // If no HTML element is found, page.$ returns null
    assert.notEqual(trashBagCount, null);
  });

  it("Should display my current 'thank you!' count", async () => {
    const page = await browser.newPage();
    await page.goto(LANDING_PAGE);

    const thankYouCount = await page.$('#thank-you-count');

    // If no HTML element is found, page.$ returns null
    assert.notEqual(thankYouCount, null);
  });

  it("Should allow users to be able to donate (if they want)", async () => {
    const page = await browser.newPage();
    await page.goto(LANDING_PAGE);

    const donation = await page.$('#donate');

    // If no HTML element is found, page.$ returns null
    assert.notEqual(donation, null);
  });
});