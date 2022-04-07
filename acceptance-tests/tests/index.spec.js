const assert = require("assert");
const puppeteer = require("puppeteer");
const HEADLESS_MODE = process.env.HEADLESS_MODE || false;
const LANDING_PAGE = process.env.LANDING_PAGE || "http://localhost:8000";
const AMAZON_ASSOCIATES_TRACKING_ID = process.env.AMAZON_ASSOCIATES_TRACKING_ID || "allbright05-20";
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
    const amazonIframeSources = frames.find(f =>
      f.url().indexOf(AMAZON_AD_SYSTEM_URL) > -1 && new URLSearchParams(f.url()).get('tracking_id') === AMAZON_ASSOCIATES_TRACKING_ID
    );

    assert.notEqual(amazonIframeSources, undefined);
  });

  it("Should link the official trash pickup video game", async () => {
    const page = await browser.newPage();
    await page.goto(LANDING_PAGE);

    const videoGameLink = await page.$('#link-to-video-game');

    // If no HTML element is found, page.$ returns null
    assert.notEqual(videoGameLink, null);
  });
});